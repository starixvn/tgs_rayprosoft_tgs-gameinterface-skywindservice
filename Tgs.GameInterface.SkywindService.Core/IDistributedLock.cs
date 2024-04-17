namespace Tgs.GameInterface.SkywindService.Core;

public interface IDistributedLock
{
	ValueTask<bool> LockingAsync(string acction);

	void Release(string acction);
}
