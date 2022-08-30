namespace Tgs.GameInterface.SkywindService.ViewModels;

public record FundResultViewModel(
    bool Status,
    string TrxId,
    string ExtTrxId,
    string OrderStatus,
    decimal Amount,
    decimal Balance);
