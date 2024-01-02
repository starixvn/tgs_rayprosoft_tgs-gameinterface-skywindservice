using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class PlayerBalanceGetService(
    IAccessToken accessToken,
    IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var token = await accessToken.GetAsync(
            apiUrl: query.ApiUrl,
            secretKey: query.SecretKey,
            username: query.Username,
            password: query.Password,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var response = await client.GetPlayerInformationAsync(
            apiUrl: query.ApiUrl,
            accessToken: token,
            playerCode: query.PlayerCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (response is null)
        {
            return new PlayerBalanceResult(
                IsPlayer: false,
                Balance: 0);
        }

        var balance = response.Balances.Where(x => x.Key.Equals(response.Currency, StringComparison.OrdinalIgnoreCase)).Single().Value.Main;

        return new PlayerBalanceResult(
            IsPlayer: true,
            Balance: balance);
    }
}
