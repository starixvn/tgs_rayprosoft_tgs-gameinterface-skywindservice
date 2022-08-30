using Tgs.GameInterface.SkywindService.Common.Protos;
using Tgs.GameInterface.SkywindService.Core.ApplicationServices;
using Tgs.MessageQueue;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.MessageHandlers;

public class AccessTokenMessageHandler : IMessageHandler
{
    private readonly ILogger<AccessTokenMessageHandler> _logger;
    private readonly ICommandService<AccessTokenCommand> _service;

    public AccessTokenMessageHandler(
        ILogger<AccessTokenMessageHandler> logger,
        ICommandService<AccessTokenCommand> service
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public ValueTask HandleAsync(
        ReadOnlyMemory<byte> data,
        CancellationToken cancellationToken)
    {
        var request = AccessTokenMessageRequest.Parser.ParseFrom(data.Span);

        if (request == null)
        {
            _logger.LogError("LogOn:{logOn} Information Received:{request}", DateTime.Now.ToString("o"), request);
            return ValueTask.CompletedTask;
        }

        _logger.LogInformation("LogOn:{logOn} Information Received:{request}", DateTime.Now.ToString("o"), request);

        var accessTokenCommand = new AccessTokenCommand(
            new Uri(request.ApiUrl),
            request.SecretKey,
            request.Username,
            request.Password);

        return _service.ExecuteAsync(
            accessTokenCommand,
            cancellationToken);
    }
}
