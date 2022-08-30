using System.Net;

namespace Tgs.GameInterface.SkywindService.Core.Exceptions;

public class NetworkException : Exception
{
    public NetworkException(HttpStatusCode statusCode, string responseMessage) : base(
        $"If the network is abnormal, please contact the engineer or game provider for assistance. " +
        $"(StatusCode:{statusCode}({(int)statusCode}),ResponseMessage:{responseMessage} )")
    {
    }
}
