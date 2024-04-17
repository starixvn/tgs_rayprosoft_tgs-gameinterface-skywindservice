using Tgs.GameInterface.SkywindService.Core.Models;

namespace Tgs.GameInterface.SkywindService.Core;

public interface IGameApiClient
{
	ValueTask<string> GetListCountriesAsync(
		Uri apiUrl,
		string accessToken,
		CancellationToken cancellationToken = default);

	ValueTask<string> GetListCurrenciesAsync(
		Uri apiUrl,
		string accessToken,
		CancellationToken cancellationToken = default);

	ValueTask<string> GetListLanguagesAsync(
		Uri apiUrl,
		string accessToken,
		CancellationToken cancellationToken = default);

	ValueTask<string> GetAccessTokenAsync(
		Uri apiUrl,
		string secretKey,
		string username,
		string password,
		CancellationToken cancellationToken = default);

	ValueTask<GetPlayerInformationResponse?> GetPlayerInformationAsync(
		Uri apiUrl,
		string accessToken,
		string playerCode,
		CancellationToken cancellationToken = default);

	ValueTask CreatePlayerAsync(
		Uri apiUrl,
		string accessToken,
		string playerCode,
		string country,
		string currency,
		string language = "EN",
		bool isTest = false,
		string status = "normal",
		CancellationToken cancellationToken = default);

	ValueTask DeleteSessionAsync(
		Uri apiUrl,
		string accessToken,
		string playerCode,
		CancellationToken cancellationToken = default);

	ValueTask<TransactionResponse?> DepositAsync(
		Uri apiUrl,
		string accessToken,
		string extTrxId,
		string playerCode,
		string currency,
		decimal amount,
		bool isTest = false,
		CancellationToken cancellationToken = default);

	ValueTask<TransactionResponse?> WithdrawalAsync(
		Uri apiUrl,
		string accessToken,
		string extTrxId,
		string playerCode,
		string currency,
		decimal amount,
		bool isTest = false,
		CancellationToken cancellationToken = default);

	ValueTask<TransactionInfoResponse?> GetTransactionStatusAsync(
		Uri apiUrl,
		string accessToken,
		string extTrxId,
		CancellationToken cancellationToken = default);

	ValueTask<GameURLBySlotResponse?> GetGameUrlAsync(
		Uri apiUrl,
		string accessToken,
		string playerCode,
		string gameCode,
		string language,
		CancellationToken cancellationToken = default);

	IAsyncEnumerable<RoundResponse> GetGameHistoryBySettleAsync(
		Uri apiUrl,
		string accessToken,
		DateTimeOffset start,
		DateTimeOffset end,
		int offset = 0,
		int limit = 20,
		string format = "string",
		CancellationToken cancellationToken = default);

	IAsyncEnumerable<RoundResponse> GetGameHistoryByUnSettleAsync(
		Uri apiUrl,
		string accessToken,
		DateTimeOffset start,
		DateTimeOffset end,
		int offset = 0,
		int limit = 20,
		string format = "string",
		CancellationToken cancellationToken = default);

	IAsyncEnumerable<Round3rdPartyResponse> GetGameHistory3rdPartyAsync(
	  Uri apiUrl,
	  string accessToken,
	  DateTimeOffset start,
	  DateTimeOffset end,
	  int offset = 0,
	  int limit = 20,
	  CancellationToken cancellationToken = default);
}
