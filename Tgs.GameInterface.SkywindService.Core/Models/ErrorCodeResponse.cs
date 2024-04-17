using System.Text.Json.Serialization;

namespace Tgs.GameInterface.SkywindService.Core.Models;

internal class ErrorCodeResponse
{
	[JsonPropertyName("code")]
	public SkywindErrorCode Code { get; set; }

	[JsonPropertyName("message")]
	public string Message { get; set; } = default!;
}
