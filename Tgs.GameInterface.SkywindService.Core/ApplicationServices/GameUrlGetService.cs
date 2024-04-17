using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class GameUrlGetService(
	IAccessToken accessToken,
	IGameApiClient client) : IGetService<GameUrlQuery, GameUrlResult>
{
	public async ValueTask<GameUrlResult> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
	{
		var token = await accessToken.GetAsync(
			apiUrl: query.ApiUrl,
			secretKey: query.SecretKey,
			username: query.Username,
			password: query.Password,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var response = await client.GetGameUrlAsync(
			apiUrl: query.ApiUrl,
			accessToken: token,
			playerCode: query.PlayerCode,
			gameCode: query.GameCode,
			language: query.Language,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new GameUrlResult(
				IsPlayer: response is not null,
				GameUrl: response?.Url ?? string.Empty);
	}
}
