using System.Runtime.CompilerServices;
using System.Text.Json;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class GameHistory3rdPartyQueryService : IQueryService<GameHistory3rdPartyQuery, GameHistoryResult>
{
    private readonly IAccessToken _accessToken;
    private readonly IGameApiClient _client;

    public GameHistory3rdPartyQueryService(
        IAccessToken accessToken,
        IGameApiClient client)
    {
        _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameHistoryResult> QueryAsync(
        GameHistory3rdPartyQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var accessToken = await _accessToken
                .GetAsync(
                    query.ApiUrl,
                    query.SecretKey,
                    query.Username,
                    query.Password,
                    cancellationToken)
                .ConfigureAwait(false);

        var rounds = await _client
                       .GetGameHistory3rdPartyAsync(
                           query.ApiUrl,
                           accessToken,
                           query.Start,
                           query.End,
                           offset: query.Offset,
                           limit: query.Limit,
                           cancellationToken: cancellationToken)
                       .ToArrayAsync(cancellationToken)
                       .ConfigureAwait(false);

        if (!rounds.Any())
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
