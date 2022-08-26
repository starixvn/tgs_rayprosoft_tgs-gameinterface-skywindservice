using System.Text.Json.Serialization;

namespace Tgs.GameInterface.SkywindService.Core.Models;

public record TransactionInfoResponse
{
    [JsonPropertyName("paymentStatus")]
    public string PaymentStatus { get; set; } = default!;
    [JsonPropertyName("status")]
    public string Status { get; set; } = default!;
    [JsonPropertyName("playerBalanceAfter")]
    public decimal PlayerBalanceAfter { get; set; }
}
