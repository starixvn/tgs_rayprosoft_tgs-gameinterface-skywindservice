using System.Net;

namespace Tgs.GameInterface.SkywindService.Core.Exceptions;

public class NetworkException(HttpStatusCode statusCode, string responseMessage) : Exception(
    $"If the network is abnormal, please contact the engineer or game provider for assistance. " +
        $"(StatusCode:{statusCode}({(int)statusCode}),ResponseMessage:{responseMessage} )")
{
}
