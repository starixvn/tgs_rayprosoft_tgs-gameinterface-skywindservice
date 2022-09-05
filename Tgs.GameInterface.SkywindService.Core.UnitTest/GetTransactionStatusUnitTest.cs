using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.SkywindService.Core.Models;

namespace Tgs.GameInterface.SkywindService.Core.UnitTest;

[TestClass]
public class GetTransactionStatusUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://localhost/");
        var accessToken = "accessToken";
        var extTrxId = "extTrxId";

        var response = new TransactionInfoResponse
        {
            PaymentStatus = "approved",
            Status = "committed",
            PlayerBalanceAfter = 1m
        };
        var url = new Uri(apiUrl, $"v1/payments/{extTrxId}");
        var content = JsonSerializer.Serialize(response);
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, url.ToString())
                    .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var atual = await client.GetTransactionStatusAsync(
              apiUrl: apiUrl,
              accessToken: accessToken,
              extTrxId: extTrxId).ConfigureAwait(false);

        Assert.IsNotNull(atual);
        Assert.AreEqual(response.Status, atual.Status);
    }

    [TestMethod]
    public async Task Order_Does_Not_Exist()
    {
        var apiUrl = new Uri("http://localhost/");
        var accessToken = "accessToken";
        var extTrxId = "extTrxId";

        var response = new TransactionInfoResponse
        {
            Status = "absent"
        };
        var url = new Uri(apiUrl, $"v1/payments/{extTrxId}");
        var content = JsonSerializer.Serialize(response);
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, url.ToString())
                    .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var atual = await client.GetTransactionStatusAsync(
              apiUrl: apiUrl,
              accessToken: accessToken,
              extTrxId: extTrxId).ConfigureAwait(false);

        Assert.IsNotNull(atual);
        Assert.AreEqual(response.Status, atual.Status);
    }
}
