namespace Tgs.GameInterface.SkywindService.Core.Exceptions;

public class AccessTokenExeption : Exception
{
    public AccessTokenExeption() : base("Failed to get AccessToken")
    {
    }
}
