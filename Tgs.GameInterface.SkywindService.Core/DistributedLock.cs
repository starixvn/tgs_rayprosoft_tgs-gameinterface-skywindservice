using System.Text.Json;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Tgs.GameInterface.SkywindService.Core;

internal class DistributedLock : IDistributedLock
{
    private readonly IDatabase _database;
    private readonly DistributedLockSettingsOptions _settings;

    public DistributedLock(
        IOptions<DistributedLockSettingsOptions> options,
        IDatabase database)
    {
        _settings = options.Value;
        _database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public async ValueTask<bool> LockingAsync(string acction)
    {
        var key = $"{_settings.LockPrefixKey}:{acction}";
        if (await _database.KeyExistsAsync(key).ConfigureAwait(false))
        {
            return false;
        }

        var value = JsonSerializer.Serialize(new
        {
            Acction = acction,
            LockTime = DateTime.UtcNow.ToString("O")
        });
        return await _database
            .StringSetAsync(
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
        _ = _database
            .KeyDelete(
                key: key,
                flags: CommandFlags.FireAndForget);
    }
}
