namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

public record PlayerBalanceQuery(
	Uri ApiUrl,
	string SecretKey,
	string Username,
	string Password,
	string PlayerCode);
