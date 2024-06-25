using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.SkywindService.Core.Exceptions;
using Tgs.GameInterface.SkywindService.Core.Models;

namespace Tgs.GameInterface.SkywindService.Core;

internal class GameApiClient(
	ILogger<GameApiClient> logger,
	HttpClient client) : IGameApiClient
{
	public async ValueTask CreatePlayerAsync(
		Uri apiUrl,
		string accessToken,
		string playerCode,
		string country,
		string currency,
		string language = "EN",
		bool isTest = false,
		string status = "normal",
		CancellationToken cancellationToken = default)
	{
		var parameters = new
		{
			code = playerCode,
			country,
			currency,
			isTest,
			language,
			status
		};

		(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await PostAsync(
						apiUrl,
						accessToken,
						"/v1/players",
						parameters,
						"CreatePlayer",
						cancellationToken)
		.ConfigureAwait(false);

		if (!IsSuccess)
		{
			logger.LogError("LogOn:{logOn}  Parameters:{parameters} StatusCode:{statusCode} Response:{responseMessage}",
				DateTime.Now.ToString("o"),
				parameters,
				$"{StatusCode}({(int)StatusCode})",
				ResponseMessage);

			try
			{
				var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage)
					?? throw new NetworkException(StatusCode, ResponseMessage);
				if (errorCodeResponse.Code != SkywindErrorCode.PLAYERS_CODE_ALREADY_EXISTS)
				{
					throw new GameProviderResponseErrorException(ResponseMessage);
				}
			}
			catch
			{
				throw new NetworkException(StatusCode, ResponseMessage);
			}
		}
	}

	public async ValueTask<TransactionResponse?> DepositAsync(
		Uri apiUrl,
		string accessToken,
		string extTrxId,
		string playerCode,
		string currency,
		decimal amount,
		bool isTest = false,
		CancellationToken cancellationToken = default)
	{
		var parameters = new
		{
			playerCode,
			currency,
			amount,
			extTrxId,
			isTest
		};

		(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await PostAsync(
						apiUrl,
						accessToken,
						"/v1/payments/transfers/in",
						parameters,
						"Deposit",
						cancellationToken)
		.ConfigureAwait(false);

		if (!IsSuccess)
		{
			logger.LogError("LogOn:{logOn}  Parameters:{parameters} StatusCode:{statusCode} Response:{responseMessage}",
				DateTime.Now.ToString("o"),
				parameters,
				$"{StatusCode}({(int)StatusCode})",
				ResponseMessage);

			try
			{
				var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage);

				return errorCodeResponse == null
					? throw new NetworkException(StatusCode, ResponseMessage)
					: errorCodeResponse.Code != SkywindErrorCode.PLAYER_NOT_FOUND
					? throw new GameProviderResponseErrorException(ResponseMessage)
					: null;
			}
			catch
			{
				throw new NetworkException(StatusCode, ResponseMessage);
			}
		}

		return StatusCode != HttpStatusCode.Created
			? throw new DuplicateTransactionException(extTrxId)
			: JsonSerializer.Deserialize<TransactionResponse>(ResponseMessage);
	}

	public async ValueTask<GameURLBySlotResponse?> GetGameUrlAsync(
		Uri apiUrl,
		string accessToken,
		string playerCode,
		string gameCode,
		string language,
		CancellationToken cancellationToken = default)
	{
		var url = new Uri(apiUrl, $"v1/players/{playerCode}/games/{gameCode}?language={language}");

		(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await GetAsync(
						url,
						accessToken,
						"GetGameUrl",
						cancellationToken)
		.ConfigureAwait(false);

		if (!IsSuccess)
		{
			logger.LogError("LogOn:{logOn}  StatusCode:{statusCode} Response:{responseMessage}",
				DateTime.Now.ToString("o"),
				$"{StatusCode}({(int)StatusCode})",
				ResponseMessage);

			try
			{
				var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage);

				return errorCodeResponse == null
					? throw new NetworkException(StatusCode, ResponseMessage)
					: errorCodeResponse.Code != SkywindErrorCode.PLAYER_NOT_FOUND
					? throw new GameProviderResponseErrorException(ResponseMessage)
					: null;
			}
			catch
			{
				throw new NetworkException(StatusCode, ResponseMessage);
			}
		}

		return JsonSerializer
			.Deserialize<GameURLBySlotResponse>(ResponseMessage);
	}

	public async ValueTask<string> GetAccessTokenAsync(
		Uri apiUrl,
		string secretKey,
		string username,
		string password,
		CancellationToken cancellationToken = default)
	{
		var parameters = new
		{
			secretKey,
			username,
			password
		};

		(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await PostAsync(
						apiUrl,
						"/v1/login",
						parameters,
						"UserLogin",
						cancellationToken)
		.ConfigureAwait(false);

		if (!IsSuccess)
		{
			logger.LogError("LogOn:{logOn}  Parameters:{parameters} StatusCode:{statusCode} Response:{responseMessage}",
				DateTime.Now.ToString("o"),
				parameters,
				$"{StatusCode}({(int)StatusCode})",
				ResponseMessage);

			try
			{
				var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage)
					?? throw new NetworkException(StatusCode, ResponseMessage);

				if (errorCodeResponse.Code != SkywindErrorCode.PLAYERS_CODE_ALREADY_EXISTS)
				{
					throw new GameProviderResponseErrorException(ResponseMessage);
				}
			}
			catch
			{
				throw new NetworkException(StatusCode, ResponseMessage);
			}
		}

		var response = JsonSerializer
						.Deserialize<UserLoginResponse>(
						ResponseMessage);

		return response?.AccessToken ?? string.Empty;
	}

	public async ValueTask<GetPlayerInformationResponse?> GetPlayerInformationAsync(
		Uri apiUrl,
		string accessToken,
		string playerCode,
		CancellationToken cancellationToken = default)

	{
		var url = new Uri(apiUrl, $"v1/players/{playerCode}");

		(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await GetAsync(
						url,
						accessToken,
						"GetPlayerInformation",
						cancellationToken)
		.ConfigureAwait(false);

		if (!IsSuccess)
		{
			logger.LogError("LogOn:{logOn}  StatusCode:{statusCode} Response:{responseMessage}",
				DateTime.Now.ToString("o"),
				$"{StatusCode}({(int)StatusCode})",
				ResponseMessage);

			try
			{
				var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage);

				return errorCodeResponse == null
					? throw new NetworkException(StatusCode, ResponseMessage)
					: errorCodeResponse.Code != SkywindErrorCode.PLAYER_NOT_FOUND
					? throw new GameProviderResponseErrorException(ResponseMessage)
					: null;
			}
			catch
			{
				throw new NetworkException(StatusCode, ResponseMessage);
			}
		}

		return JsonSerializer
			.Deserialize<GetPlayerInformationResponse>(ResponseMessage);
	}

	public async ValueTask<TransactionResponse?> WithdrawalAsync(
		Uri apiUrl,
		string accessToken,
		string extTrxId,
		string playerCode,
		string currency,
		decimal amount,
		bool isTest = false,
		CancellationToken cancellationToken = default)
	{
		var parameters = new
		{
			playerCode,
			currency,
			amount,
			extTrxId,
			isTest
		};

		(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await PostAsync(
						apiUrl,
						accessToken,
						"/v1/payments/transfers/out",
						parameters,
						"Withdrawal",
						cancellationToken)
		.ConfigureAwait(false);

		if (!IsSuccess)
		{
			logger.LogError("LogOn:{logOn}  Parameters:{parameters} StatusCode:{statusCode} Response:{responseMessage}",
				DateTime.Now.ToString("o"),
				parameters,
				$"{StatusCode}({(int)StatusCode})",
				ResponseMessage);

			try
			{
				var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage);

				return errorCodeResponse == null
					? throw new NetworkException(StatusCode, ResponseMessage)
					: errorCodeResponse.Code != SkywindErrorCode.PLAYER_NOT_FOUND
					? throw new GameProviderResponseErrorException(ResponseMessage)
					: null;
			}
			catch
			{
				throw new NetworkException(StatusCode, ResponseMessage);
			}
		}

		return StatusCode != HttpStatusCode.Created
			? throw new DuplicateTransactionException(extTrxId)
			: JsonSerializer.Deserialize<TransactionResponse>(ResponseMessage);
	}

	public async ValueTask<string> GetListCountriesAsync(
		Uri apiUrl,
		string accessToken,
		CancellationToken cancellationToken = default)

	{
		var url = new Uri(apiUrl, $"v1/countries");

		(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await GetAsync(
						url,
						accessToken,
						"GetListCountries",
						cancellationToken)
		.ConfigureAwait(false);

		if (!IsSuccess)
		{
			logger.LogError("LogOn:{logOn}  StatusCode:{statusCode} Response:{responseMessage}",
				DateTime.Now.ToString("o"),
				$"{StatusCode}({(int)StatusCode})",
				ResponseMessage);
		}

		return ResponseMessage;
	}

	public async ValueTask<string> GetListCurrenciesAsync(
		Uri apiUrl,
		string accessToken,
		CancellationToken cancellationToken = default)

	{
		var url = new Uri(apiUrl, $"v1/currencies");

		(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await GetAsync(
						url,
						accessToken,
						"GetListCurrencies",
						cancellationToken)
		.ConfigureAwait(false);

		if (!IsSuccess)
		{
			logger.LogError("LogOn:{logOn}  StatusCode:{statusCode} Response:{responseMessage}",
				DateTime.Now.ToString("o"),
				$"{StatusCode}({(int)StatusCode})",
				ResponseMessage);
		}

		return ResponseMessage;
	}

	public async ValueTask<string> GetListLanguagesAsync(
		Uri apiUrl,
		string accessToken,
		CancellationToken cancellationToken = default)

	{
		var url = new Uri(apiUrl, $"v1/languages");

		(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await GetAsync(
						url,
						accessToken,
						"GetListLanguages",
						cancellationToken)
		.ConfigureAwait(false);

		if (!IsSuccess)
		{
			logger.LogError("LogOn:{logOn}  StatusCode:{statusCode} Response:{responseMessage}",
				DateTime.Now.ToString("o"),
				$"{StatusCode}({(int)StatusCode})",
				ResponseMessage);
		}

		return ResponseMessage;
	}

	public async IAsyncEnumerable<RoundResponse> GetGameHistoryBySettleAsync(
		Uri apiUrl,
		string accessToken,
		DateTimeOffset start,
		DateTimeOffset end,
		int offset = 0,
		int limit = 20,
		string format = "string",
		[EnumeratorCancellation] CancellationToken cancellationToken = default)

	{
		var query = HttpUtility.ParseQueryString(string.Empty);
		query["offset"] = offset.ToString();
		query["limit"] = limit.ToString();
		query["format"] = format;
		query["ts__gte"] = start.ToString(@"yyyy-MM-dd\THH:mm:ss.fffK");
		query["ts__lte"] = end.ToString(@"yyyy-MM-dd\THH:mm:ss.fffK");
		query["finished"] = "true";

		var url = new Uri(apiUrl, $"v1/history/game?{query}");

		(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await GetAsync(
						url,
						accessToken,
						"GetGameHistoryBySettle",
						cancellationToken)
		.ConfigureAwait(false);

		if (!IsSuccess)
		{
			logger.LogError("LogOn:{logOn}  StatusCode:{statusCode} Response:{responseMessage}",
				DateTime.Now.ToString("o"),
				$"{StatusCode}({(int)StatusCode})",
				ResponseMessage);

			try
			{
				var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage)
					?? throw new NetworkException(StatusCode, ResponseMessage);

				throw new GameProviderResponseErrorException(ResponseMessage);
			}
			catch
			{
				throw new NetworkException(StatusCode, ResponseMessage);
			}
		}

		var rounds = JsonSerializer.Deserialize<IEnumerable<RoundResponse>>(ResponseMessage) ?? [];

		if (!rounds.Any())
		{
			yield break;
		}

		foreach (var round in rounds)
		{
			yield return round;
		}
	}

	public async IAsyncEnumerable<RoundResponse> GetGameHistoryByUnSettleAsync(
		Uri apiUrl,
		string accessToken,
		DateTimeOffset start,
		DateTimeOffset end,
		int offset = 0,
		int limit = 20,
		string format = "string",
		[EnumeratorCancellation] CancellationToken cancellationToken = default)

	{
		var query = HttpUtility.ParseQueryString(string.Empty);
		query["offset"] = offset.ToString();
		query["limit"] = limit.ToString();
		query["format"] = format;
		query["firstTs__gte"] = start.ToString(@"yyyy-MM-dd\THH:mm:ss.fffK");
		query["firstTs__lte"] = end.ToString(@"yyyy-MM-dd\THH:mm:ss.fffK");
		query["finished"] = "false";

		var url = new Uri(apiUrl, $"v1/history/game?{query}");

		(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await GetAsync(
						url,
						accessToken,
						"GetGameHistoryBySettle",
						cancellationToken)
		.ConfigureAwait(false);

		if (!IsSuccess)
		{
			logger.LogError("LogOn:{logOn}  StatusCode:{statusCode} Response:{responseMessage}",
				DateTime.Now.ToString("o"),
				$"{StatusCode}({(int)StatusCode})",
				ResponseMessage);

			try
			{
				var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage)
					?? throw new NetworkException(StatusCode, ResponseMessage);

				throw new GameProviderResponseErrorException(ResponseMessage);
			}
			catch
			{
				throw new NetworkException(StatusCode, ResponseMessage);
			}
		}

		var rounds = JsonSerializer.Deserialize<IEnumerable<RoundResponse>>(ResponseMessage) ?? [];

		if (!rounds.Any())
		{
			yield break;
		}

		foreach (var round in rounds)
		{
			yield return round;
		}
	}

	public async IAsyncEnumerable<Round3rdPartyResponse> GetGameHistory3rdPartyAsync(
		Uri apiUrl,
		string accessToken,
		DateTimeOffset start,
		DateTimeOffset end,
		int offset = 0,
		int limit = 20,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)

	{
		var query = HttpUtility.ParseQueryString(string.Empty);
		query["offset"] = offset.ToString();
		query["limit"] = limit.ToString();
		query["insertedAt__gt"] = start.ToString(@"yyyy-MM-dd\THH:mm:ss.fffK");
		query["insertedAt__lt"] = end.ToString(@"yyyy-MM-dd\THH:mm:ss.fffK");
		query["finished"] = "true";

		var url = new Uri(apiUrl, $"v1/history/external?{query}");

		(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await GetAsync(
						url,
						accessToken,
						"GetGameHistory3rdParty",
						cancellationToken)
		.ConfigureAwait(false);

		if (!IsSuccess)
		{
			logger.LogError("LogOn:{logOn}  StatusCode:{statusCode} Response:{responseMessage}",
				DateTime.Now.ToString("o"),
				$"{StatusCode}({(int)StatusCode})",
				ResponseMessage);

			try
			{
				var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage)
					?? throw new NetworkException(StatusCode, ResponseMessage);

				throw new GameProviderResponseErrorException(ResponseMessage);
			}
			catch
			{
				throw new NetworkException(StatusCode, ResponseMessage);
			}
		}

		var rounds = JsonSerializer.Deserialize<IEnumerable<Round3rdPartyResponse>>(ResponseMessage) ?? [];

		if (!rounds.Any())
		{
			yield break;
		}

		foreach (var round in rounds)
		{
			yield return round;
		}
	}

	public async ValueTask<TransactionInfoResponse?> GetTransactionStatusAsync(
		Uri apiUrl,
		string accessToken,
		string extTrxId,
		CancellationToken cancellationToken = default)

	{
		var url = new Uri(apiUrl, $"v1/payments/{extTrxId}");

		(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await GetAsync(
						url,
						accessToken,
						"GetTransactionStatus",
						cancellationToken)
		.ConfigureAwait(false);

		if (!IsSuccess)
		{
			logger.LogError("LogOn:{logOn}  StatusCode:{statusCode} Response:{responseMessage}",
				DateTime.Now.ToString("o"),
				$"{StatusCode}({(int)StatusCode})",
				ResponseMessage);

			try
			{
				var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage);

				return errorCodeResponse == null
					? throw new NetworkException(StatusCode, ResponseMessage)
					: errorCodeResponse.Code != SkywindErrorCode.PLAYER_NOT_FOUND
					? throw new GameProviderResponseErrorException(ResponseMessage)
					: null;
			}
			catch
			{
				throw new NetworkException(StatusCode, ResponseMessage);
			}
		}

		return JsonSerializer
			.Deserialize<TransactionInfoResponse>(ResponseMessage);
	}

	public async ValueTask DeleteSessionAsync(
		Uri apiUrl,
		string accessToken,
		string playerCode,
		CancellationToken cancellationToken = default)
	{
		var url = new Uri(apiUrl, $"v1/players/{playerCode}/session");

		(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage) = await DeleteAsync(
						url,
						accessToken,
						"DeleteSession",
						cancellationToken)
		.ConfigureAwait(false);

		if (!IsSuccess)
		{
			try
			{
				var errorCodeResponse = JsonSerializer.Deserialize<ErrorCodeResponse>(ResponseMessage)
					?? throw new NetworkException(StatusCode, ResponseMessage);

				if (errorCodeResponse.Code != SkywindErrorCode.PLAYER_NOT_FOUND)
				{
					throw new GameProviderResponseErrorException(ResponseMessage);
				}
			}
			catch
			{
				throw new NetworkException(StatusCode, ResponseMessage);
			}
		}
	}

	private async ValueTask<(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage)> PostAsync(Uri apiUrl, string path, dynamic parameters, string logPrefix, CancellationToken cancellationToken = default)
	{
		logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

		var sw = Stopwatch.StartNew();
		var url = new Uri(apiUrl, path);

		HttpContent content = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

		var httpResponseMessage = await client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);

		using (var stream = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result, Encoding.UTF8))
		{
			responseMessage = await stream
				.ReadToEndAsync(
				cancellationToken: cancellationToken)
				.ConfigureAwait(false) ?? string.Empty;
		}

		var logEvent = new GameApiLogEvent
		{
			Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
			RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
			ResponseHeaders = httpResponseMessage.Headers.ToString(),
			ResponseContent = responseMessage.NoLongerThan(4096),
			ResponseCode = (short)httpResponseMessage.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};
		logger.LogInformation("{logEvent}", JsonSerializer.Serialize(logEvent));

		return (httpResponseMessage.IsSuccessStatusCode, httpResponseMessage.StatusCode, responseMessage);
	}

	private async ValueTask<(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage)> PostAsync(Uri apiUrl, string accessToken, string path, dynamic parameters, string logPrefix, CancellationToken cancellationToken = default)
	{
		logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

		var sw = Stopwatch.StartNew();
		var url = new Uri(apiUrl, path);

		HttpContent content = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

		client.DefaultRequestHeaders.Add("X-ACCESS-TOKEN", accessToken);
		var httpResponseMessage = await client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);

		using (var stream = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result, Encoding.UTF8))
		{
			responseMessage = await stream
				.ReadToEndAsync(
				cancellationToken: cancellationToken)
				.ConfigureAwait(false) ?? string.Empty;
		}

		var logEvent = new GameApiLogEvent
		{
			Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
			RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
			ResponseHeaders = httpResponseMessage.Headers.ToString(),
			ResponseContent = responseMessage.NoLongerThan(4096),
			ResponseCode = (short)httpResponseMessage.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};
		logger.LogInformation("{logEvent}", JsonSerializer.Serialize(logEvent));

		return (httpResponseMessage.IsSuccessStatusCode, httpResponseMessage.StatusCode, responseMessage);
	}

	private async ValueTask<(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage)> GetAsync(Uri apiUrl, string accessToken, string logPrefix, CancellationToken cancellationToken = default)
	{
		logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

		var sw = Stopwatch.StartNew();

		client.DefaultRequestHeaders.Add("X-ACCESS-TOKEN", accessToken);
		var httpResponseMessage = await client.GetAsync(apiUrl, cancellationToken).ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);

		using (var stream = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result, Encoding.UTF8))
		{
			responseMessage = await stream
				.ReadToEndAsync(
				cancellationToken: cancellationToken)
				.ConfigureAwait(false) ?? string.Empty;
		}

		var logEvent = new GameApiLogEvent
		{
			Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
			RequestContent = string.Empty,
			ResponseHeaders = httpResponseMessage.Headers.ToString(),
			ResponseContent = responseMessage.NoLongerThan(4096),
			ResponseCode = (short)httpResponseMessage.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};
		logger.LogInformation("{logEvent}", JsonSerializer.Serialize(logEvent));

		return (httpResponseMessage.IsSuccessStatusCode, httpResponseMessage.StatusCode, responseMessage);
	}

	private async ValueTask<(bool IsSuccess, HttpStatusCode StatusCode, string ResponseMessage)> DeleteAsync(Uri apiUrl, string accessToken, string logPrefix, CancellationToken cancellationToken = default)
	{
		logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

		var sw = Stopwatch.StartNew();

		client.DefaultRequestHeaders.Add("X-ACCESS-TOKEN", accessToken);
		var httpResponseMessage = await client.DeleteAsync(apiUrl, cancellationToken).ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);

		using (var stream = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result, Encoding.UTF8))
		{
			responseMessage = await stream
				.ReadToEndAsync(
				cancellationToken: cancellationToken)
				.ConfigureAwait(false) ?? string.Empty;
		}

		var logEvent = new GameApiLogEvent
		{
			Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
			RequestContent = string.Empty,
			ResponseHeaders = httpResponseMessage.Headers.ToString(),
			ResponseContent = responseMessage.NoLongerThan(4096),
			ResponseCode = (short)httpResponseMessage.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};
		logger.LogInformation("{logEvent}", JsonSerializer.Serialize(logEvent));

		return (httpResponseMessage.IsSuccessStatusCode, httpResponseMessage.StatusCode, responseMessage);
	}
}
