using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.SkywindService.Core.Models;

namespace Tgs.GameInterface.SkywindService.Core.UnitTest;

[TestClass]
public class GetGameHistory3rdpartyUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://localhost/");
        var accessToken = "accessToken";
        var start = DateTimeOffset.Now.AddHours(-1);
        var end = DateTimeOffset.Now;

        var response = new Round3rdPartyResponse[] {
            new(){
                ExtTrxId="dobjuQ5UxtsAAAMIdobjpCQ9iFg",
                Bet= 5000,
                Win=0,
                Currency="IDR",
                BrandId="dR8NNv39",
                PlayerCode= "4uidrpaurbt3",
                RoundId= "eErobEwVV",
                GameCode= "itg_goldenox",
                BalanceBefore= 107800,
                BalanceAfter= 102800,
                IsTest= false,
                Revenue= 5000,
                InsertedAt=DateTimeOffset.Parse( "2022-11-14T14:24:23.471Z"),
                ProviderSpecificData= null,
                GameProviderCode= "ITG",
                Finished= true,
                EventId= 0 }};

        var url = new Uri(apiUrl, $"v1/history/external");
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
                .GetGameHistory3rdPartyAsync(
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
