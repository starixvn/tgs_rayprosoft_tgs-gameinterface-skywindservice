using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class PlayerBalanceGetService : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    private readonly IAccessToken _accessToken;
    private readonly IGameApiClient _client;

    public PlayerBalanceGetService(
        IAccessToken accessToken,
        IGameApiClient client)
    {
        _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var accessToken = await _accessToken
                .GetAsync(
                    query.ApiUrl,
                    query.SecretKey,
                    query.Username,
                    query.Password,
                    cancellationToken)
                .ConfigureAwait(false);

        var response = await _client
                        .GetPlayerInformationAsync(
                            query.ApiUrl,
                            accessToken,
                            query.PlayerCode,
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
