using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class FundStatusGetService : IGetService<FundStatusQuery, FundStatusResult>
{
    private readonly IAccessToken _accessToken;
    private readonly IGameApiClient _client;

    public FundStatusGetService(
        IAccessToken accessToken,
        IGameApiClient client)
    {
        _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundStatusResult> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
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
                        .GetTransactionStatusAsync(
                            query.ApiUrl,
                            accessToken,
                            query.ExtTrxId,
                            cancellationToken: cancellationToken)
                        .ConfigureAwait(false);

        return response is null
            ? new FundStatusResult(
                IsTransaction: false,
                Status: string.Empty)
            : new FundStatusResult(
            IsTransaction: (!response.Status.Equals("absent", StringComparison.OrdinalIgnoreCase)),
            Status: response.Status);
    }
}
