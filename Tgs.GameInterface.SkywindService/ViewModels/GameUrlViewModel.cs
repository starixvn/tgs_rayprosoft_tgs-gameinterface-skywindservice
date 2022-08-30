namespace Tgs.GameInterface.SkywindService.ViewModels;

public record GameUrlViewModel
{
    public Uri ApiUrl { get; init; } = default!;
    public string SecretKey { get; init; } = default!;
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string PlayerCode { get; init; } = default!;
    public string GameCode { get; init; } = default!;
    public string Language { get; init; } = default!;
}
