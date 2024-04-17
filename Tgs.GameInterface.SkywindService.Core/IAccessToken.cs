namespace Tgs.GameInterface.SkywindService.Core;

public interface IAccessToken
{
	ValueTask CreateAsync(
		Uri apiUrl,
		string secretKey,
		string username,
		string password,
		CancellationToken cancellationToken = default);

	ValueTask<string> GetAsync(
		Uri apiUrl,
		string secretKey,
		string username,
		string password,
		CancellationToken cancellationToken = default);

	ValueTask<TimeSpan?> GetTTLAsync(
		string username,
		CancellationToken cancellationToken = default);
}
