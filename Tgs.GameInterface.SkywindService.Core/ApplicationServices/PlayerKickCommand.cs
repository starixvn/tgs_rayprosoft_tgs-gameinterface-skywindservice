namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

public record PlayerKickCommand(
	Uri ApiUrl,
	string SecretKey,
	string Username,
	string Password,
	string PlayerCode);
