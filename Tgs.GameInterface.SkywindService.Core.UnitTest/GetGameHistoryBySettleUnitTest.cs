using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.SkywindService.Core.Models;

namespace Tgs.GameInterface.SkywindService.Core.UnitTest;

[TestClass]
public class GetGameHistoryBySettleUnitTest
{
	[TestMethod]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("http://localhost/");
		var accessToken = "accessToken";
		var start = DateTimeOffset.Now.AddHours(-1);
		var end = DateTimeOffset.Now;

		var response = new RoundResponse[] {
			new(){
				Finished= true,
				Revenue= 0.23m,
				Currency="CNY",
				Debit= null,
				Credit= null,
				RoundId= "pLoNpG5Qe",
				BrandId= "ABwZ8pRb",
				PlayerCode= "4ucmqplxb029",
				GameCode= "sw_888t",
				Device= "web",
				Win= 0.02M,
				Bet= 0.25M,
				BalanceBefore= 998.31M,
				BalanceAfter= 998.08M,
				TotalEvents= "1",
				FirstTs=DateTimeOffset.Parse( "2022-08-25T03:12:02.690Z"),
				Ts=DateTimeOffset.Parse( "2022-08-25T03:12:02.690Z"),
				IsTest= false,
				InsertedAt= DateTimeOffset.Parse("2022-08-25T03:12:03.205Z"),
				RecoveryType= null,
				TotalJpContribution= 0,
				TotalJpWin= 0 }};

		var url = new Uri(apiUrl, $"v1/history/game");
		var content = JsonSerializer.Serialize(response);
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Get, url.ToString())
							   .Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var atual = await client
				.GetGameHistoryBySettleAsync(
					apiUrl: apiUrl,
					accessToken: accessToken,
					start: start,
					end: end)
				.ToArrayAsync()
				.ConfigureAwait(false);

		Assert.IsNotNull(atual);
		// Assert.AreEqual(response.Url, atual.Url);
	}
}
