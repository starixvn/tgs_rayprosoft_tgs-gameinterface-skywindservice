namespace Tgs.GameInterface.SkywindService.ViewModels;

public record ListLanguagesViewModel
{
	public Uri ApiUrl { get; init; } = default!;
	public string AccessToken { get; init; } = default!;
}
