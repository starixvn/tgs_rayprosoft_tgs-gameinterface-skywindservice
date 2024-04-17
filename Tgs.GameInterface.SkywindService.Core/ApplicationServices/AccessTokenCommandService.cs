using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class AccessTokenCommandService(
	ILogger<AccessTokenCommandService> logger,
	IDistributedLock distributedLock,
	IAccessToken accessToken) : ICommandService<AccessTokenCommand>
{
	public async ValueTask ExecuteAsync(AccessTokenCommand command, CancellationToken cancellationToken = default)
	{
		var acction = $"accesstoken:{command.Username}";
		if (!await distributedLock
		.LockingAsync(acction)
		.ConfigureAwait(false))
		{
			logger.LogError("LogOn:{logOn} | This access token has been locked. | AccessTokenCommand:{command}", DateTime.UtcNow.ToString("O"), command);
			return;
		}

		try
		{
			var timeSpan = await accessToken.GetTTLAsync(
				command.Username,
				cancellationToken).ConfigureAwait(false);
			if (timeSpan.HasValue)
			{
				if (timeSpan.Value.Minutes > 1)
				{
					return;
				}
			}

			await accessToken
				.CreateAsync(
					command.ApiUrl,
					command.SecretKey,
					command.Username,
					command.Password,
					cancellationToken)
				.ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "LogOn:{logOn} | exception error | AccessTokenCommand:{command}", DateTime.UtcNow.ToString("O"), command);
		}
		finally
		{
			distributedLock.Release(acction);
		}
	}
}
