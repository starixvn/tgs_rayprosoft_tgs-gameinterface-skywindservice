using System.Text.Json.Serialization;

namespace Tgs.GameInterface.SkywindService.Core.Models;

public record TransactionResponse
{
    [JsonPropertyName("trxId")]
    public string TrxId { get; set; } = default!;
    [JsonPropertyName("extTrxId")]
    public string ExtTrxId { get; set; } = default!;
    [JsonPropertyName("brandId")]
    public string BrandId { get; set; } = default!;
    [JsonPropertyName("playerCode")]
    public string PlayerCode { get; set; } = default!;
    [JsonPropertyName("orderId")]
    public string? OrderId { get; set; } = default;
    [JsonPropertyName("orderType")]
    public string OrderType { get; set; } = default!;
    [JsonPropertyName("orderDate")]
    public DateTimeOffset OrderDate { get; set; }
    [JsonPropertyName("orderInfo")]
    public string? OrderInfo { get; set; } = default;
    [JsonPropertyName("orderStatus")]
    public string OrderStatus { get; set; } = default!;
    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = default!;
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    [JsonPropertyName("playerBalanceAfter")]
    public decimal PlayerBalanceAfter { get; set; }
    [JsonPropertyName("startDate")]
    public DateTimeOffset StartDate { get; set; }
    [JsonPropertyName("endDate")]
    public DateTimeOffset EndDate { get; set; }
    [JsonPropertyName("paymentMethodCode")]
    public string? PaymentMethodCode { get; set; } = default;
    [JsonPropertyName("isTest")]
    public bool IsTest { get; set; }
}
