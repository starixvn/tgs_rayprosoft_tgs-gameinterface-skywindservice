using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class PlayerKickCommandService(
	IAccessToken accessToken,
	IGameApiClient client) : ICommandService<PlayerKickCommand>
{
	public async ValueTask ExecuteAsync(PlayerKickCommand command, CancellationToken cancellationToken = default)
	{
		var token = await accessToken.GetAsync(
			apiUrl: command.ApiUrl,
			secretKey: command.SecretKey,
			username: command.Username,
			password: command.Password,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		await client.DeleteSessionAsync(
			apiUrl: command.ApiUrl,
			accessToken: token,
			playerCode: command.PlayerCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}
}
