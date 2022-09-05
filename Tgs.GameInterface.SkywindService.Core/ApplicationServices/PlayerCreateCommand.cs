namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

public record PlayerCreateCommand(
    Uri ApiUrl,
    string SecretKey,
    string Username,
    string Password,
    string PlayerCode,
    string Currency);
