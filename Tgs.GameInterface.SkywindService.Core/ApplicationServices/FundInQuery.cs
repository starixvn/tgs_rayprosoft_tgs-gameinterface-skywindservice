namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

public record FundInQuery(
    Uri ApiUrl,
    string SecretKey,
    string Username,
    string Password,
    string ExtTrxId,
    string PlayerCode,
    string Currency,
    decimal Amount);
