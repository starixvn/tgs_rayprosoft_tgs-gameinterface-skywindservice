namespace Tgs.GameInterface.SkywindService.ViewModels;

public record ListCurrenciesViewModel
{
    public Uri ApiUrl { get; init; } = default!;
    public string AccessToken { get; init; } = default!;
}
