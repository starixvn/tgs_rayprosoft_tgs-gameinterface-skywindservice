using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class FundInGetService(
    IAccessToken accessToken,
    IGameApiClient client) : IGetService<FundInQuery, FundInResult>
{
    public async ValueTask<FundInResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
    {
        var token = await accessToken.GetAsync(
            apiUrl: query.ApiUrl,
            secretKey: query.SecretKey,
            username: query.Username,
            password: query.Password,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var response = await client.WithdrawalAsync(
            apiUrl: query.ApiUrl,
            accessToken: token,
            extTrxId: query.ExtTrxId,
            playerCode: query.PlayerCode,
            currency: query.Currency,
            amount: query.Amount,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new FundInResult(
                Status: response is not null,
                TrxId: response?.TrxId ?? string.Empty,
                ExtTrxId: response?.ExtTrxId ?? string.Empty,
                OrderStatus: response?.OrderStatus ?? string.Empty,
                Amount: response?.Amount ?? 0,
                Balance: response?.PlayerBalanceAfter ?? 0);
    }
}
