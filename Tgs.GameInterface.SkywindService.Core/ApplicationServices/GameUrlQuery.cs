namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

public record GameUrlQuery(
	Uri ApiUrl,
	string SecretKey,
	string Username,
	string Password,
	string PlayerCode,
	string GameCode,
	string Language);
