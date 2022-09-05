using System.Reflection;
using StackExchange.Redis;
using Tgs.GameInterface.SkywindService.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfaceCore(
        this IServiceCollection services,
        Action<GameInterfaceSettingOptions, IServiceProvider> gameInterfaceSettingOptions,
        Action<DistributedLockSettingsOptions, IServiceProvider> distributedLockSettingsOptions,
        Func<IServiceProvider, IDatabase> redisDatabaseResolver)
        => services
            .AddOptions<GameInterfaceSettingOptions>()
            .Configure(gameInterfaceSettingOptions)
            .Services
            .AddOptions<DistributedLockSettingsOptions>()
            .Configure(distributedLockSettingsOptions)
            .Services
            .RegisterAllServiceUtilityTypes(
                Assembly.GetExecutingAssembly(),
                ServiceLifetime.Transient)
            .AddHttpClient<IGameApiClient, GameApiClient>()
            .Services
            .AddScoped<IGamePublishMessage, GamePublishMessage>()
            .AddScoped<IDistributedLock>((sp) => ActivatorUtilities.CreateInstance<DistributedLock>(sp, redisDatabaseResolver(sp)))
            .AddScoped<IAccessToken>((sp) => ActivatorUtilities.CreateInstance<AccessToken>(sp, redisDatabaseResolver(sp), TimeSpan.FromMinutes(15)));
}
