using StackExchange.Redis;
using Tgs.GameInterface.SkywindService.Common.Protos;
using Tgs.GameInterface.SkywindService.Core.Exceptions;

namespace Tgs.GameInterface.SkywindService.Core;

public class AccessToken : IAccessToken
{
    private readonly IDatabase _database;
    private readonly IGameApiClient _client;
    private readonly IGamePublishMessage _gamePublishMessage;
    private readonly TimeSpan _expiry;
    private readonly string _rediskey = "gp:gameinterface:skywind:accesstoken:{0}";

    public AccessToken(
        IDatabase database,
        IGameApiClient client,
        IGamePublishMessage gamePublishMessage,
        TimeSpan expiry)
    {
        _database = database ?? throw new ArgumentNullException(nameof(database));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _gamePublishMessage = gamePublishMessage ?? throw new ArgumentNullException(nameof(gamePublishMessage));
        _expiry = expiry;
    }

    public async ValueTask CreateAsync(
        Uri apiUrl,
        string secretKey,
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        var accessToken = await _client
                .GetAccessTokenAsync(
                    apiUrl,
                    secretKey,
                    username,
                    password,
                    cancellationToken)
                .ConfigureAwait(false);

        if (string.IsNullOrEmpty(accessToken))
        {
            throw new AccessTokenExeption();
        }

        var key = string.Format(_rediskey, username);

        _ = await _database
                .StringSetAsync(
                    key,
                    accessToken,
                    _expiry)
                .ConfigureAwait(false);
    }

    public async ValueTask<string> GetAsync(
        Uri apiUrl,
        string secretKey,
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        var key = string.Format(_rediskey, username);

        var accessToken = !await _database
            .KeyExistsAsync(
                key)
            .ConfigureAwait(false)
            ? await _client
                .GetAccessTokenAsync(
                    apiUrl,
                    secretKey,
                    username,
                    password,
                    cancellationToken)
                .ConfigureAwait(false)
            : (string?)await _database.StringGetAsync(key).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new AccessTokenExeption();
        }

        var request = new AccessTokenMessageRequest()
        {
            ApiUrl = apiUrl.ToString(),
            SecretKey = secretKey,
            Username = username,
            Password = password
        };

        await _gamePublishMessage
            .AccessTokenPublishMessageAsync(
                request,
                cancellationToken)
            .ConfigureAwait(!false);

        return accessToken;
    }

    public async ValueTask<TimeSpan?> GetTTLAsync(
        string username,
        CancellationToken cancellationToken = default)
        => !await _database
            .KeyExistsAsync(
                string.Format(_rediskey, username))
            .ConfigureAwait(false)
            ? null
            : await _database
            .KeyTimeToLiveAsync(
                string.Format(_rediskey, username))
            .ConfigureAwait(false);
}
