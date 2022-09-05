using System.Diagnostics;

namespace Tgs.GameInterface.SkywindService;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.SkywindService.DiagnosticsInterceptor");
}
