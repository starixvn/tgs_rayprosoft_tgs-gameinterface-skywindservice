using System.Runtime.CompilerServices;
using System.Text.Json;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class GameHistoryBySettleQueryService : IQueryService<GameHistoryBySettleQuery, GameHistoryResult>
{
    private readonly IAccessToken _accessToken;
    private readonly IGameApiClient _client;

    public GameHistoryBySettleQueryService(
        IAccessToken accessToken,
        IGameApiClient client)
    {
        _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameHistoryResult> QueryAsync(
        GameHistoryBySettleQuery query,
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
                       .GetGameHistoryBySettleAsync(
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
                round.Finished,
                round.Revenue,
                round.Currency,
                round.Debit,
                round.Credit,
                round.RoundId,
                round.BrandId,
                round.PlayerCode,
                round.GameCode,
                round.Device,
                round.Win,
                round.Bet,
                round.BalanceBefore,
                round.BalanceAfter,
                round.TotalEvents,
                round.FirstTs,
                round.Ts,
                round.IsTest,
                round.InsertedAt,
                round.RecoveryType,
                round.TotalJpContribution,
                round.TotalJpWin,
                JsonSerializer.Serialize(round));
        }
    }
}
