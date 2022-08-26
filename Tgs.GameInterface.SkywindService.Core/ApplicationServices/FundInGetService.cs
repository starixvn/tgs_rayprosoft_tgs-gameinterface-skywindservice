using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class FundInGetService : IGetService<FundInQuery, FundInResult>
{
    private readonly IAccessToken _accessToken;
    private readonly IGameApiClient _client;

    public FundInGetService(
        IAccessToken accessToken,
        IGameApiClient client)
    {
        _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundInResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
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
                        .WithdrawalAsync(
                            query.ApiUrl,
                            accessToken,
                            query.ExtTrxId,
                            query.PlayerCode,
                            query.Currency,
                            query.Amount,
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
