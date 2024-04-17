namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

public record GameHistoryBySettleQuery(
	Uri ApiUrl,
	string SecretKey,
	string Username,
	string Password,
	DateTimeOffset Start,
	DateTimeOffset End,
	int Offset,
	int Limit);
