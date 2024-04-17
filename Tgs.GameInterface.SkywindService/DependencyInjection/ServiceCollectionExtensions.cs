using Tgs.GameInterface.SkywindService;
using Tgs.GameInterface.SkywindService.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameInterfaceService(
		this IServiceCollection services,
		Action<GameInterfaceSettingOptions, IServiceProvider> gameInterfaceSettingOptions,
		Action<DistributedLockSettingsOptions, IServiceProvider> distributedLockSettingsOptions,
		Action<RedisOptions, IServiceProvider> redisDBconfigure)
		=> services
			.AddOptions<RedisOptions>()
			.Configure(redisDBconfigure)
			.Services
			.AddSingleton<RedisConnectionManager>()
			.AddGameInterfaceCore(
			gameInterfaceSettingOptions,
			distributedLockSettingsOptions,
			(sp) => sp.GetRequiredService<RedisConnectionManager>().Database);
}
