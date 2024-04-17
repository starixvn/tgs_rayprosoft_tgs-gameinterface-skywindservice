using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class FundStatusGetService(
	IAccessToken accessToken,
	IGameApiClient client) : IGetService<FundStatusQuery, FundStatusResult>
{
	public async ValueTask<FundStatusResult> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
	{
		var token = await accessToken.GetAsync(
			apiUrl: query.ApiUrl,
			secretKey: query.SecretKey,
			username: query.Username,
			password: query.Password,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var response = await client.GetTransactionStatusAsync(
			apiUrl: query.ApiUrl,
			accessToken: token,
			extTrxId: query.ExtTrxId,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return response is null
			? new FundStatusResult(
				IsTransaction: false,
				Status: string.Empty)
			: new FundStatusResult(
			IsTransaction: !response.Status.Equals("absent", StringComparison.OrdinalIgnoreCase),
			Status: response.Status);
	}
}
