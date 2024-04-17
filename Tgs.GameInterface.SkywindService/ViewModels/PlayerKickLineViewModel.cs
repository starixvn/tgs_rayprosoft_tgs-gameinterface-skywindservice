namespace Tgs.GameInterface.SkywindService.ViewModels;

public record PlayerKickViewModel
{
	public Uri ApiUrl { get; init; } = default!;
	public string SecretKey { get; init; } = default!;
	public string Username { get; init; } = default!;
	public string Password { get; init; } = default!;
	public string PlayerCode { get; init; } = default!;
}
