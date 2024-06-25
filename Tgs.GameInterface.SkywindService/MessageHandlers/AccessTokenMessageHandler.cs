using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text;
using Tgs.GameInterface.SkywindService.Common.Protos;
using Tgs.GameInterface.SkywindService.Core.ApplicationServices;
using Tgs.MessageQueue;
using Tgs.ServiceUtility;
using Valhalla.MessageQueue;

namespace Tgs.GameInterface.SkywindService.MessageHandlers;

public class AccessTokenMessageHandler(
	ILogger<AccessTokenMessageHandler> logger,
	ICommandService<AccessTokenCommand> service) : IMessageHandler<AccessTokenMessageRequest>
{
	public ValueTask HandleAsync(string subject, AccessTokenMessageRequest data, IEnumerable<MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
	{
		logger.LogInformation("LogOn:{logOn} Subject:{subject} Information Request:{data}",
			DateTime.Now.ToString("o"),
			subject,
			data);

		return service.ExecuteAsync(
			command: new AccessTokenCommand(
				ApiUrl: new Uri(data.ApiUrl),
				SecretKey: data.SecretKey,
				Username: data.Username,
				Password: data.Password),
			cancellationToken: cancellationToken);
	}
}
