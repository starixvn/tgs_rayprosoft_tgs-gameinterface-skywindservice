using System.Text.Json;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Tgs.GameInterface.SkywindService.Core;

internal class DistributedLock(
	IOptions<DistributedLockSettingsOptions> options,
	IDatabase database) : IDistributedLock
{
	private readonly DistributedLockSettingsOptions _settings = options.Value;

	public async ValueTask<bool> LockingAsync(string acction)
	{
		var key = $"{_settings.LockPrefixKey}:{acction}";
		if (await database.KeyExistsAsync(key).ConfigureAwait(false))
		{
			return false;
		}

		var value = JsonSerializer.Serialize(new
		{
			Acction = acction,
			LockTime = DateTime.UtcNow.ToString("O")
		});
		return await database.StringSetAsync(
			key: key,
			value: value,
			expiry: TimeSpan.FromMinutes(_settings.LockMinutes),
			when: When.NotExists,
			flags: CommandFlags.None)
			.ConfigureAwait(false);
	}

	public void Release(string acction)
	{
		var key = $"{_settings.LockPrefixKey}:{acction}";
		_ = database.KeyDelete(
			key: key,
			flags: CommandFlags.FireAndForget);
	}
}
