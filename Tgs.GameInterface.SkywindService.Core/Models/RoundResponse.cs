using System.Text.Json.Serialization;

namespace Tgs.GameInterface.SkywindService.Core.Models;

public record RoundResponse
{
	[JsonPropertyName("finished")]
	public bool Finished { get; set; }
	[JsonPropertyName("revenue")]
	public decimal? Revenue { get; set; }
	[JsonPropertyName("currency")]
	public string Currency { get; set; } = default!;
	[JsonPropertyName("debit")]
	public decimal? Debit { get; set; }
	[JsonPropertyName("credit")]
	public decimal? Credit { get; set; }
	[JsonPropertyName("roundId")]
	public string RoundId { get; set; } = default!;
	[JsonPropertyName("brandId")]
	public string BrandId { get; set; } = default!;
	[JsonPropertyName("playerCode")]
	public string PlayerCode { get; set; } = default!;
	[JsonPropertyName("gameCode")]
	public string GameCode { get; set; } = default!;
	[JsonPropertyName("device")]
	public string Device { get; set; } = default!;
	[JsonPropertyName("win")]
	public decimal? Win { get; set; }
	[JsonPropertyName("bet")]
	public decimal Bet { get; set; }
	[JsonPropertyName("balanceBefore")]
	public decimal BalanceBefore { get; set; }
	[JsonPropertyName("balanceAfter")]
	public decimal BalanceAfter { get; set; }
	[JsonPropertyName("totalEvents")]
	public string TotalEvents { get; set; } = default!;
	[JsonPropertyName("firstTs")]
	public DateTimeOffset FirstTs { get; set; }
	[JsonPropertyName("ts")]
	public DateTimeOffset? Ts { get; set; }
	[JsonPropertyName("isTest")]
	public bool IsTest { get; set; }
	[JsonPropertyName("insertedAt")]
	public DateTimeOffset InsertedAt { get; set; }
	[JsonPropertyName("recoveryType")]
	public string? RecoveryType { get; set; } = default;
	[JsonPropertyName("totalJpContribution")]
	public decimal? TotalJpContribution { get; set; }
	[JsonPropertyName("totalJpWin")]
	public decimal? TotalJpWin { get; set; }
}
