using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class GameURLByLiveGetService : IGetService<GameURLByLiveQuery, GameURLResult>
{
    private readonly IAccessToken _accessToken;
    private readonly IGameApiClient _client;

    public GameURLByLiveGetService(
        IAccessToken accessToken,
        IGameApiClient client)
    {
        _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameURLResult> GetAsync(GameURLByLiveQuery query, CancellationToken cancellationToken = default)
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
            .GameURLByLiveAsync(
                query.ApiUrl,
                accessToken,
                query.PlayerCode,
                query.AuthToken,
                query.Language,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new GameURLResult(
                IsPlayer: response is not null,
                GameUrl: response?.Url ?? string.Empty);
    }
}
