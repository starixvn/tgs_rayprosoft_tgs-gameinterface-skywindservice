using System.Diagnostics;

namespace Tgs.GameInterface.SkywindService.Client;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.SkywindService.Client.DiagnosticsInterceptor");
}
