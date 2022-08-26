using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class GameURLBySlotGetService : IGetService<GameURLBySlotQuery, GameURLResult>
{
    private readonly IAccessToken _accessToken;
    private readonly IGameApiClient _client;

    public GameURLBySlotGetService(
        IAccessToken accessToken,
        IGameApiClient client)
    {
        _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameURLResult> GetAsync(GameURLBySlotQuery query, CancellationToken cancellationToken = default)
    {
        var accessToken = await _accessToken
                .GetAsync(
                    query.ApiUrl,
                    query.SecretKey,
                    query.Username,
                    query.Password,
                    cancellationToken)
                .ConfigureAwait(false);

        var response = await _client
            .GameURLBySlotAsync(
                query.ApiUrl,
                accessToken,
                query.PlayerCode,
                query.GameCode,
                query.Language,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new GameURLResult(
                IsPlayer: response is not null,
                GameUrl: response?.Url ?? string.Empty);
    }
}
