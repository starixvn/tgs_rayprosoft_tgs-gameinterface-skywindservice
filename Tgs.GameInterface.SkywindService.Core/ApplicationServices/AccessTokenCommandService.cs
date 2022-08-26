using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class AccessTokenCommandService : ICommandService<AccessTokenCommand>
{
    private readonly ILogger<AccessTokenCommandService> _logger;
    private readonly IDistributedLock _distributedLock;
    private readonly IAccessToken _accessToken;

    public AccessTokenCommandService(
        ILogger<AccessTokenCommandService> logger,
        IDistributedLock distributedLock,
        IAccessToken accessToken)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _distributedLock = distributedLock ?? throw new ArgumentNullException(nameof(distributedLock));
        _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
    }

    public async ValueTask ExecuteAsync(AccessTokenCommand command, CancellationToken cancellationToken = default)
    {
        var acction = $"accesstoken:{command.Username}";
        if (!await _distributedLock
        .LockingAsync(acction)
        .ConfigureAwait(false))
        {
            _logger.LogError("LogOn:{logOn} | This access token has been locked. | AccessTokenCommand:{command}", DateTime.UtcNow.ToString("O"), command);
            return;
        }

        try
        {
            var timeSpan = await _accessToken.GetTTLAsync(
                command.Username,
                cancellationToken).ConfigureAwait(false);
            if (timeSpan.HasValue)
            {
                if (timeSpan.Value.Minutes > 1)
                {
                    return;
                }
            }
            await _accessToken
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
            _logger.LogError(ex, "LogOn:{logOn} | exception error | AccessTokenCommand:{command}", DateTime.UtcNow.ToString("O"), command);
        }
        finally
        {
            _distributedLock.Release(acction);
        }
    }
}
