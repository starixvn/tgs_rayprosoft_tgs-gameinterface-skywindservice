namespace Tgs.GameInterface.SkywindService.Core;

public class DistributedLockSettingsOptions
{
    public string LockPrefixKey { get; init; } = default!;

    public double LockMinutes { get; init; }
}
