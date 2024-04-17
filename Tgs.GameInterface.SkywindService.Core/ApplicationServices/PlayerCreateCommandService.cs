using Microsoft.Extensions.Options;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

internal class PlayerCreateCommandService(
	IOptions<GameInterfaceSettingOptions> options,
	IAccessToken accessToken,
	IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
	private readonly GameInterfaceSettingOptions _gameInterfaceSetting = options.Value;

	public async ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
	{
		var currency = _gameInterfaceSetting.CurrencyMappings
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

		var token = await accessToken.GetAsync(
			apiUrl: command.ApiUrl,
			secretKey: command.SecretKey,
			username: command.Username,
			password: command.Password,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		await client.CreatePlayerAsync(
			apiUrl: command.ApiUrl,
			accessToken: token,
			playerCode: command.PlayerCode,
			country: currency.CountryCode,
			currency: currency.CurrencyCode,
			language: currency.LanguageCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}
}
