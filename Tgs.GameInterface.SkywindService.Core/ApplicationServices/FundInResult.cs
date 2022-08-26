namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

public record FundInResult(
    bool Status,
    string TrxId,
    string ExtTrxId,
    string OrderStatus,
    decimal Amount,
    decimal Balance);
