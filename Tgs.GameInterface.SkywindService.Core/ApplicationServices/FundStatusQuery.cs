namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

public record FundStatusQuery(
	Uri ApiUrl,
	string SecretKey,
	string Username,
	string Password,
	string ExtTrxId);
