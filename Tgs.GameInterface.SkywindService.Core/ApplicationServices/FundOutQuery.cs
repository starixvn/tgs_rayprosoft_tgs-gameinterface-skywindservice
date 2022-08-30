namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

public record FundOutQuery(
    Uri ApiUrl,
    string SecretKey,
    string Username,
    string Password,
    string ExtTrxId,
    string PlayerCode,
    string Currency,
    decimal Amount);
