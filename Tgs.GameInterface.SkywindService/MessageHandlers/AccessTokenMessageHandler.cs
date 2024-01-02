using Tgs.GameInterface.SkywindService.Common.Protos;
using Tgs.GameInterface.SkywindService.Core.ApplicationServices;
using Tgs.MessageQueue;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.MessageHandlers;

public class AccessTokenMessageHandler(
    ILogger<AccessTokenMessageHandler> logger,
    ICommandService<AccessTokenCommand> service) : IMessageHandler
{
    public ValueTask HandleAsync(
        ReadOnlyMemory<byte> data,
        CancellationToken cancellationToken)
    {
        var request = AccessTokenMessageRequest.Parser.ParseFrom(data.Span);

        if (request == null)
        {
            logger.LogError("LogOn:{logOn} Information Received:{request}", DateTime.Now.ToString("o"), request);
            return ValueTask.CompletedTask;
        }

        logger.LogInformation("LogOn:{logOn} Information Received:{request}", DateTime.Now.ToString("o"), request);

        var accessTokenCommand = new AccessTokenCommand(
            new Uri(request.ApiUrl),
            request.SecretKey,
            request.Username,
            request.Password);

        return service.ExecuteAsync(
            accessTokenCommand,
            cancellationToken);
    }
}
