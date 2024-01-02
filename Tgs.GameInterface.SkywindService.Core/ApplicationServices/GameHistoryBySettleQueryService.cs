using System.Runtime.CompilerServices;
using System.Text.Json;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class GameHistoryBySettleQueryService(
    IAccessToken accessToken,
    IGameApiClient client) : IQueryService<GameHistoryBySettleQuery, GameHistoryResult>
{
    public async IAsyncEnumerable<GameHistoryResult> QueryAsync(
        GameHistoryBySettleQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var token = await accessToken.GetAsync(
            apiUrl: query.ApiUrl,
            secretKey: query.SecretKey,
            username: query.Username,
            password: query.Password,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var rounds = await client
            .GetGameHistoryBySettleAsync(
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
                ExternalRoundId: round.RoundId,
                ExternalGameId: round.GameCode,
                Finished: round.Finished,
                ExternalCreatedOn: round.FirstTs,
                ExternalClosedOn: round.Ts!.Value,
                Bet: round.Bet,
                ValidBet: round.Bet,
                PlayerWinLoss: round.Win.GetValueOrDefault(0) - round.Bet,
                Turnover: round.Bet,
                Settle: round.Win.GetValueOrDefault(0),
                AdditionalData: JsonSerializer.Serialize(round));
        }
    }
}
