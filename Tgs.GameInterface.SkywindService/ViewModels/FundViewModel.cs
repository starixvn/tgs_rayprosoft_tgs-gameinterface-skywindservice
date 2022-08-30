namespace Tgs.GameInterface.SkywindService.ViewModels;

public record FundViewModel
{
    public Uri ApiUrl { get; init; } = default!;
    public string SecretKey { get; init; } = default!;
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string ExtTrxId { get; init; } = default!;
    public string PlayerCode { get; init; } = default!;
    public string Currency { get; init; } = default!;
    public decimal Amount { get; init; }
}
