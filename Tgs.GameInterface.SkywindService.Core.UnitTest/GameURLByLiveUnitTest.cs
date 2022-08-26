using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.SkywindService.Core.Models;

namespace Tgs.GameInterface.SkywindService.Core.UnitTest;

[TestClass]
public class GameURLByLiveUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://localhost/");
        var accessToken = "accessToken";
        var playerCode = "test";
        var authToken = "authToken";
        var language = "zh-cn";

        var response = new GameURLBySlotResponse
        {
            Url = "http://localhost/",
            Token = Guid.NewGuid().ToString("N"),
            Currency = "CNY"
        };
        var url = new Uri(apiUrl, $"v1/players/{playerCode}/games/sw_ro_a01bac?language={language}");
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
                .GameURLByLiveAsync(
                    apiUrl: apiUrl,
                    accessToken: accessToken,
                    playerCode: playerCode,
                    authToken: authToken,
                    language: language)
                .ConfigureAwait(false);

        Assert.IsNotNull(atual);
        Assert.AreEqual(response.Url, atual.Url);
    }
}
