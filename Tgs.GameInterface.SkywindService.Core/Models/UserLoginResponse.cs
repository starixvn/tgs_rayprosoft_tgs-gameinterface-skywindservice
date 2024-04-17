using System.Text.Json.Serialization;

namespace Tgs.GameInterface.SkywindService.Core.Models;

internal record UserLoginResponse
{
	[JsonPropertyName("key")]
	public string Key { get; set; } = default!;
	[JsonPropertyName("username")]
	public string UserName { get; set; } = default!;
	[JsonPropertyName("accessToken")]
	public string AccessToken { get; set; } = default!;
}
