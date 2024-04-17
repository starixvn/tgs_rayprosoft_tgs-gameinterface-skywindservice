namespace Tgs.GameInterface.SkywindService.Core;

public class GameInterfaceSettingOptions
{
	public NatsSetting NatsSetting { get; set; } = default!;

	public IEnumerable<CurrencyMapping> CurrencyMappings { get; set; } = default!;
}
