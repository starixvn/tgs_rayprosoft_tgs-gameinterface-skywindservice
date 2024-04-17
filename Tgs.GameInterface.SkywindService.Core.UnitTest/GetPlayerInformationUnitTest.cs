using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.SkywindService.Core.Models;

namespace Tgs.GameInterface.SkywindService.Core.UnitTest;

[TestClass]
public class CheckTransferByTransactionIdUnitTest
{
	[TestMethod]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("http://localhost/");
		var accessToken = "accessToken";
		var playerCode = "test";

		var response = new GetPlayerInformationResponse
		{
			PlayerCode = playerCode,
			Id = "vRXWg21q",
			Status = "normal",
			Currency = "CNY",
			Country = "CN",
			Language = "zh-cn",
			GameGroup = null,
			AgentId = null,
			IsTest = false,
			LastLogin = null,
			CreatedAt = DateTimeOffset.Parse("2022-08-24T05:24:58.135Z"),
			UpdatedAt = DateTimeOffset.Parse("2022-08-24T05:24:58.135Z"),
			DeactivatedAt = null,
			BrandId = "ABwZ8pRb",
			BrandTitle = "PGT_brand",
			Nickname = null,
			IsVip = false,
			DefaultGameGroup = null,
			IsBlocked = false,
			IsPrivateChatBlock = false,
			IsPublicChatBlock = false,
			IsTracked = false,
			HasWarn = false,
			NicknameChangeAttempts = 0,
			Balances = new Dictionary<string, GetPlayerInformationBalance> {
				{ "CNY",new GetPlayerInformationBalance{ Main=0} }
			},
			IsOnline = false
		};
		var url = new Uri(apiUrl, $"v1/players/{playerCode}");
		var content = JsonSerializer.Serialize(response);
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Get, url.ToString())
					.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var atual = await client.GetPlayerInformationAsync(
			  apiUrl: apiUrl,
			  accessToken: accessToken,
			  playerCode: playerCode).ConfigureAwait(false);

		Assert.IsNotNull(atual);
		Assert.AreEqual(playerCode, atual.PlayerCode);
	}
}
