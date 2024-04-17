namespace Tgs.GameInterface.SkywindService.ViewModels;

public record RoundResultViewModel(
	string PlayerCode,
	string ExternalRoundId,
	string ExternalGameId,
	bool Finished,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	decimal Bet,
	decimal ValidBet,
	decimal PlayerWinLoss,
	decimal Turnover,
	decimal Settle,
	string AdditionalData);
