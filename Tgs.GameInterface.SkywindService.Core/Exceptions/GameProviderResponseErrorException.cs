namespace Tgs.GameInterface.SkywindService.Core.Exceptions;

public class GameProviderResponseErrorException(string responseMessage) : Exception(
   $"The game manufacturer responds with an error code, please contact the engineer or game supplier for assistance." +
	   $"({responseMessage})")
{
}
