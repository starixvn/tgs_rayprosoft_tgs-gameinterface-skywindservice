using System.Text.Json.Serialization;

namespace Tgs.GameInterface.SkywindService.Core.Models;

public record Round3rdPartyResponse
{
    [JsonPropertyName("extTrxId")]
    public string ExtTrxId { get; set; } = default!;
    [JsonPropertyName("bet")]
    public decimal Bet { get; set; }
    [JsonPropertyName("win")]
    public decimal Win { get; set; }
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = default!;
    [JsonPropertyName("brandId")]
    public string BrandId { get; set; } = default!;
    [JsonPropertyName("playerCode")]
    public string PlayerCode { get; set; } = default!;
    [JsonPropertyName("roundId")]
    public string RoundId { get; set; } = default!;
    [JsonPropertyName("gameCode")]
    public string GameCode { get; set; } = default!;
    [JsonPropertyName("balanceBefore")]
    public decimal BalanceBefore { get; set; }
    [JsonPropertyName("balanceAfter")]
    public decimal BalanceAfter { get; set; }
    [JsonPropertyName("isTest")]
    public bool IsTest { get; set; }
    [JsonPropertyName("revenue")]
    public decimal Revenue { get; set; }
    [JsonPropertyName("insertedAt")]
    public DateTimeOffset InsertedAt { get; set; }
    [JsonPropertyName("providerSpecificData")]
    public decimal? ProviderSpecificData { get; set; }
    [JsonPropertyName("gameProviderCode")]
    public string GameProviderCode { get; set; } = default!;
    [JsonPropertyName("finished")]
    public bool Finished { get; set; }
    [JsonPropertyName("eventId")]
    public int EventId { get; set; }
}
