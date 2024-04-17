using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.SkywindService.Core.Models;

namespace Tgs.GameInterface.SkywindService.Core.UnitTest;

[TestClass]
public class CreatePlayerUnitTest
{
	[TestMethod]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("http://localhost/");
		var accessToken = "accessToken";
		var playerCode = "test";
		var country = "CN";
		var currency = "CNY";

		var response = new CreatePlayerResponse
		{
			PlayerCode = "test",
			Id = "yRgy7dM3",
			Status = "normal",
			Currency = "CNY",
			Country = "CN",
			Language = "zh-cn",
			GameGroup = null,
			AgentId = null,
			IsTest = false,
			LastLogin = null,
			CreatedAt = DateTimeOffset.Parse("2022-08-24T09:15:48.674Z"),
			UpdatedAt = DateTimeOffset.Parse("2022-08-24T09:15:48.674Z"),
			BrandId = "ABwZ8pRb",
			BrandTitle = "PGT_brand",
			NickName = null,
			IsVip = false,
			IsBlocked = false,
			IsPrivateChatBlock = false,
			IsPublicChatBlock = false,
			IsTracked = false,
			HasWarn = false,
			NicknameChangeAttempts = 0
		};
		var url = new Uri(apiUrl, $"/v1/players");
		var content = JsonSerializer.Serialize(response);
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, url.ToString())
						.Respond(HttpStatusCode.Created, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		await client.CreatePlayerAsync(
			  apiUrl: apiUrl,
			  accessToken: accessToken,
			  playerCode: playerCode,
			  country: country,
			 currency: currency).ConfigureAwait(false);
	}
}
