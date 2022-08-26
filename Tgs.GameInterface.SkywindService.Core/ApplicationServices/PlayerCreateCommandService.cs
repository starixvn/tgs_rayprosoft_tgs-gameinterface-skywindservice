using Microsoft.Extensions.Options;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class PlayerCreateCommandService : ICommandService<PlayerCreateCommand>
{
    private readonly GameInterfaceSettingOptions _gameInterfaceSetting;
    private readonly IAccessToken _accessToken;
    private readonly IGameApiClient _client;

    public PlayerCreateCommandService(
        IOptions<GameInterfaceSettingOptions> options,
        IAccessToken accessToken,
        IGameApiClient client)
    {
        _gameInterfaceSetting = options.Value;
        _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
    {
        var currency = _gameInterfaceSetting
                        .CurrencyMappings
                        .Where(
                            x => x.CurrencyCode.Equals(command.Currency, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault() ??
                        new CurrencyMapping()
                        {
                            CurrencyCode = command.Currency,
                            CountryCode = "CN",
                            LanguageCode = "zh-cn",
                            DisplayName = "China"
                        };

        var accessToken = await _accessToken
                .GetAsync(
                    command.ApiUrl,
                    command.SecretKey,
                    command.Username,
                    command.Password,
                    cancellationToken)
                .ConfigureAwait(false);

        await _client
                .CreatePlayerAsync(
                    command.ApiUrl,
                    accessToken,
                    command.PlayerCode,
                    currency.CountryCode,
                    currency.CurrencyCode,
                    currency.LanguageCode,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
    }
}
