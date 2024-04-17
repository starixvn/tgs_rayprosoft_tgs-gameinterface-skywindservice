using System.Runtime.CompilerServices;
using System.Text.Json;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class GameHistory3rdPartyQueryService(
	IAccessToken accessToken,
	IGameApiClient client) : IQueryService<GameHistory3rdPartyQuery, GameHistoryResult>
{
	public async IAsyncEnumerable<GameHistoryResult> QueryAsync(
		GameHistory3rdPartyQuery query,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var token = await accessToken.GetAsync(
			apiUrl: query.ApiUrl,
			secretKey: query.SecretKey,
			username: query.Username,
			password: query.Password,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var rounds = await client.GetGameHistory3rdPartyAsync(
			apiUrl: query.ApiUrl,
			accessToken: token,
			start: query.Start,
			end: query.End,
			offset: query.Offset,
			limit: query.Limit,
			cancellationToken: cancellationToken)
			.ToArrayAsync(cancellationToken)
			.ConfigureAwait(false);

		if (rounds.Length == 0)
		{
			yield break;
		}

		foreach (var round in rounds)
		{
			yield return new GameHistoryResult(
				PlayerCode: round.PlayerCode,
				ExternalRoundId: $"{round.RoundId}_{round.EventId}",
				ExternalGameId: round.GameCode,
				Finished: round.Finished,
				ExternalCreatedOn: round.InsertedAt,
				ExternalClosedOn: round.InsertedAt,
				Bet: round.Bet,
				ValidBet: round.Bet,
				PlayerWinLoss: round.Win - round.Bet,
				Turnover: round.Bet,
				Settle: round.Win,
				AdditionalData: JsonSerializer.Serialize(round));
		}
	}
}
