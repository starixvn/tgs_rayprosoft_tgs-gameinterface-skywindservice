using System.Net.Sockets;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tgs.GameInterface.SkywindService.Core;

namespace Tgs.GameInterface.SkywindService.Controllers;

[Route("[controller]")]
[ApiController]
public class InfoController(
    IConfiguration configuration,
    IOptions<GameInterfaceSettingOptions> gameInterfaceSettingOptions,
    IOptions<DistributedLockSettingsOptions> distributedLockSettingsOptions) : ControllerBase
{
    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="_hostingEnvironment">The hosting environment.</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<object> GetAsync([FromServices] IWebHostEnvironment _hostingEnvironment)
    {
        var hostName = Dns.GetHostName();
        var hostEntry = await Dns.GetHostEntryAsync(hostName);
        var hostIp = Array.Find(hostEntry.AddressList,
            x => x.AddressFamily == AddressFamily.InterNetwork);
        return new
        {
            Data = new
            {
                Environment.MachineName,
                HostName = hostName,
                HostIp = hostIp?.ToString() ?? string.Empty,
                Environment = _hostingEnvironment.EnvironmentName,
                OsVersion = $"{Environment.OSVersion}",
                Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                ProcessCount = Environment.ProcessorCount,
                ReleaseNamespace = configuration.GetValue<string>("RELEASE_NAMESPACE"),
                PodIPAddress = configuration.GetValue<string>("POD_IP"),
                GameInterfaceSettingOptions = gameInterfaceSettingOptions.Value,
                DistributedLockSettingsOptions = distributedLockSettingsOptions.Value

            }
        };
    }
}