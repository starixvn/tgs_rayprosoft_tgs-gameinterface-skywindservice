using System.Text.Json.Serialization;

namespace Tgs.GameInterface.SkywindService.Core.Models;

public record GetPlayerInformationResponse
{
	[JsonPropertyName("code")]
	public string PlayerCode { get; set; } = default!;
	[JsonPropertyName("id")]
	public string Id { get; set; } = default!;
	[JsonPropertyName("status")]
	public string Status { get; set; } = default!;
	[JsonPropertyName("country")]
	public string Country { get; set; } = default!;
	[JsonPropertyName("currency")]
	public string Currency { get; set; } = default!;
	[JsonPropertyName("language")]
	public string Language { get; set; } = default!;
	[JsonPropertyName("gameGroup")]
	public string? GameGroup { get; set; } = default;
	[JsonPropertyName("agentId")]
	public string? AgentId { get; set; } = default;

	[JsonPropertyName("isTest")]
	public bool IsTest { get; set; }
	[JsonPropertyName("lastLogin")]
	public DateTimeOffset? LastLogin { get; set; }
	[JsonPropertyName("createdAt")]
	public DateTimeOffset CreatedAt { get; set; }
	[JsonPropertyName("updatedAt")]
	public DateTimeOffset UpdatedAt { get; set; }
	[JsonPropertyName("deactivatedAt")]
	public string? DeactivatedAt { get; set; } = default;
	[JsonPropertyName("brandId")]
	public string BrandId { get; set; } = default!;
	[JsonPropertyName("brandTitle")]
	public string BrandTitle { get; set; } = default!;
	[JsonPropertyName("nickname")]
	public string? Nickname { get; set; } = default;
	[JsonPropertyName("isVip")]
	public bool IsVip { get; set; }
	[JsonPropertyName("defaultGameGroup")]
	public string? DefaultGameGroup { get; set; } = default;
	[JsonPropertyName("isBlocked")]
	public bool IsBlocked { get; set; }
	[JsonPropertyName("isPrivateChatBlock")]
	public bool IsPrivateChatBlock { get; set; }
	[JsonPropertyName("isPublicChatBlock")]
	public bool IsPublicChatBlock { get; set; }
	[JsonPropertyName("isTracked")]
	public bool IsTracked { get; set; }
	[JsonPropertyName("hasWarn")]
	public bool HasWarn { get; set; }
	[JsonPropertyName("nicknameChangeAttempts")]
	public int NicknameChangeAttempts { get; set; }
	[JsonPropertyName("balances")]
	public Dictionary<string, GetPlayerInformationBalance> Balances { get; set; } = default!;
	[JsonPropertyName("isOnline")]
	public bool IsOnline { get; set; } = default!;
}

public class GetPlayerInformationBalance
{
	[JsonPropertyName("main")]
	public decimal Main { get; set; }
}
