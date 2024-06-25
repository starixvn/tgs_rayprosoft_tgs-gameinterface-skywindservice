using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tgs.GameInterface.SkywindService.Common.Protos;
using Tgs.MessageQueue;
using Valhalla.MessageQueue;

namespace Tgs.GameInterface.SkywindService.Core;

internal class GamePublishMessage(
	ILogger<GamePublishMessage> logger,
	IOptions<GameInterfaceSettingOptions> options,
	IMessageSender messageSender) : IGamePublishMessage
{
	private readonly GameInterfaceSettingOptions _gameInterfaceSetting = options.Value;

	public async ValueTask AccessTokenPublishMessageAsync(AccessTokenMessageRequest request, CancellationToken cancellationToken = default)
	{
		await messageSender.PublishAsync(
			subject: _gameInterfaceSetting.NatsSetting.Accesstoken.Subject,
			data: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		logger.LogInformation("LogOn:{logOn} Subject:{subject} Information Sending:{request}",
			DateTime.Now.ToString("o"),
			_gameInterfaceSetting.NatsSetting.Accesstoken.Subject,
			request);
	}
}
