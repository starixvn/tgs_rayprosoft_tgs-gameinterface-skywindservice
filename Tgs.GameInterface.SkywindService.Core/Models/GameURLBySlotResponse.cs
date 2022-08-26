using System.Text.Json.Serialization;

namespace Tgs.GameInterface.SkywindService.Core.Models;

public record GameURLBySlotResponse
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = default!;
    [JsonPropertyName("token")]
    public string Token { get; set; } = default!;
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = default!;
}
