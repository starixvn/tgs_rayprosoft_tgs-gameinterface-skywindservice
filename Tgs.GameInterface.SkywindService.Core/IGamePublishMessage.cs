using Tgs.GameInterface.SkywindService.Common.Protos;

namespace Tgs.GameInterface.SkywindService.Core;

public interface IGamePublishMessage
{
    ValueTask AccessTokenPublishMessageAsync(AccessTokenMessageRequest request, CancellationToken cancellationToken = default);
}
