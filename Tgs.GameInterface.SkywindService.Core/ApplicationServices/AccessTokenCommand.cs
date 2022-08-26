namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

public record AccessTokenCommand(
    Uri ApiUrl,
    string SecretKey,
    string Username,
    string Password);
