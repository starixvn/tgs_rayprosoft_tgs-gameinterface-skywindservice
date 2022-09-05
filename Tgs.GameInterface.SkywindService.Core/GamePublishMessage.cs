using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tgs.GameInterface.SkywindService.Common.Protos;
using Tgs.MessageQueue;

namespace Tgs.GameInterface.SkywindService.Core;

internal class GamePublishMessage : IGamePublishMessage
{
    private readonly ILogger<GamePublishMessage> _logger;
    private readonly GameInterfaceSettingOptions _gameInterfaceSetting;
    private readonly IMessageSender _messageSender;

    public GamePublishMessage(
        ILogger<GamePublishMessage> logger,
        IOptions<GameInterfaceSettingOptions> options,
        IMessageSender messageSender)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gameInterfaceSetting = options.Value;
        _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
    }

    public async ValueTask AccessTokenPublishMessageAsync(AccessTokenMessageRequest request, CancellationToken cancellationToken = default)
    {
        await _messageSender
            .PublishAsync(
                subject: _gameInterfaceSetting.NatsSetting.Accesstoken.Subject,
                data: request.ToByteArray(),
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("LogOn:{logOn} Subject:{subject} Information Sending:{request}",
            DateTime.Now.ToString("o"),
            _gameInterfaceSetting.NatsSetting.Accesstoken.Subject,
            request);
    }
}
