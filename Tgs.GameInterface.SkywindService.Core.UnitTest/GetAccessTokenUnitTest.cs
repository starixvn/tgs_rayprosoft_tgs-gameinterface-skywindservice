using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.SkywindService.Core.Models;

namespace Tgs.GameInterface.SkywindService.Core.UnitTest;

[TestClass]
public class GetAccessTokenUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://localhost/");
        var secretKey = "secretKey";
        var username = "username";
        var password = "password";

        var response = new UserLoginResponse
        {
            Key = Guid.NewGuid().ToString("N"),
            UserName = username,
            AccessToken = Guid.NewGuid().ToString("N")
        };
        var url = new Uri(apiUrl, $"/v1/login");
        var content = JsonSerializer.Serialize(response);
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.Created, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var atual = await client.GetAccessTokenAsync(
              apiUrl: apiUrl,
              secretKey: secretKey,
              username: username,
              password: password).ConfigureAwait(false);

        Assert.IsNotNull(atual);
        Assert.AreEqual(response.AccessToken, atual);
    }
}
