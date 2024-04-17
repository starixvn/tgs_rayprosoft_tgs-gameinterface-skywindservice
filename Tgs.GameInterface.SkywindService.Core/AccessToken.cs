using StackExchange.Redis;
using Tgs.GameInterface.SkywindService.Common.Protos;
using Tgs.GameInterface.SkywindService.Core.Exceptions;

namespace Tgs.GameInterface.SkywindService.Core;

public class AccessToken(
	IDatabase database,
	IGameApiClient client,
	IGamePublishMessage gamePublishMessage,
	TimeSpan expiry) : IAccessToken
{
	private readonly string _rediskey = "gp:gameinterface:skywind:accesstoken:{0}";

	public async ValueTask CreateAsync(
		Uri apiUrl,
		string secretKey,
		string username,
		string password,
		CancellationToken cancellationToken = default)
	{
		var accessToken = await client
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

		_ = await database
				.StringSetAsync(
					key,
					accessToken,
					expiry)
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

		var accessToken = !await database
			.KeyExistsAsync(
				key)
			.ConfigureAwait(false)
			? await client
				.GetAccessTokenAsync(
					apiUrl,
					secretKey,
					username,
					password,
					cancellationToken)
				.ConfigureAwait(false)
			: (string?)await database.StringGetAsync(key).ConfigureAwait(false);
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

		await gamePublishMessage
			.AccessTokenPublishMessageAsync(
				request,
				cancellationToken)
			.ConfigureAwait(!false);

		return accessToken;
	}

	public async ValueTask<TimeSpan?> GetTTLAsync(
		string username,
		CancellationToken cancellationToken = default)
		=> !await database
			.KeyExistsAsync(
				string.Format(_rediskey, username))
			.ConfigureAwait(false)
			? null
			: await database
			.KeyTimeToLiveAsync(
				string.Format(_rediskey, username))
			.ConfigureAwait(false);
}
