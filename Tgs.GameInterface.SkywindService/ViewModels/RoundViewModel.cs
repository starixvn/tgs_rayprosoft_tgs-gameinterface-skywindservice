using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.SkywindService.ViewModels;

public record RoundViewModel
{
    public Uri ApiUrl { get; init; } = default!;
    public string SecretKey { get; init; } = default!;
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
    public DateTimeOffset Start { get; init; } = default!;
    public DateTimeOffset End { get; init; } = default!;
    public int Offset { get; init; } = default!;
    [Range(minimum: 20, maximum: 100)]
    public int Limit { get; init; } = default!;
}
