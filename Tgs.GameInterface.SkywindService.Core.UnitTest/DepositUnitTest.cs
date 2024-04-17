using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.SkywindService.Core.Models;

namespace Tgs.GameInterface.SkywindService.Core.UnitTest;

[TestClass]
public class DepositUnitTest
{
	[TestMethod]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("http://localhost/");
		var accessToken = "accessToken";

		var extTrxId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
		var playerCode = "test";
		var currency = "CNY";
		var amount = 1m;

		var response = new TransactionResponse
		{
			TrxId = "TpNrQkdRnjsAAAMFTpNrXEcYH6k=",
			ExtTrxId = extTrxId,
			BrandId = "ABwZ8pRb",
			PlayerCode = playerCode,
			OrderId = null,
			OrderType = "transfer_in",
			OrderDate = DateTimeOffset.Parse("2022-08-24T06:39:04.028Z"),
			OrderInfo = null,
			OrderStatus = "approved",
			CurrencyCode = "CNY",
			Amount = amount,
			PlayerBalanceAfter = 1,
			StartDate = DateTimeOffset.Parse("2022-08-24T06:39:04.028Z"),
			EndDate = DateTimeOffset.Parse("2022-08-24T06:39:04.028Z"),
			PaymentMethodCode = null,
			IsTest = false
		};
		var url = new Uri(apiUrl, $"/v1/payments/transfers/in");
		var content = JsonSerializer.Serialize(response);
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, url.ToString())
							.Respond(HttpStatusCode.Created, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var atual = await client.DepositAsync(
			  apiUrl: apiUrl,
			  accessToken: accessToken,
			  extTrxId: extTrxId,
			  playerCode: playerCode,
			  currency: currency,
			  amount: amount).ConfigureAwait(false);

		Assert.IsNotNull(atual);
		Assert.AreEqual(playerCode, atual.PlayerCode);
		Assert.AreEqual(extTrxId, atual.ExtTrxId);
	}
}
