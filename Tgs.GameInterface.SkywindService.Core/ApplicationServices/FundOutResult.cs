namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

public record FundOutResult(
    bool Status,
    string TrxId,
    string ExtTrxId,
    string OrderStatus,
    decimal Amount,
    decimal Balance);
