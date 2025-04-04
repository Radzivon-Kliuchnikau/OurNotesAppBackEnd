using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace OurNotesAppBackEnd.Tests.Integration.Infrastructure;

public static class TestAuthHelper
{
    public static async Task<HttpClient> GetAuthenticatedClient(WebApplicationFactory<IApiMarker> factory)
    {
        var cookieContainer = new CookieContainer();
        var innerCustomHandler = new HttpClientHandler { CookieContainer = cookieContainer };
        var delegationgHandler = new PassThroughHandler(innerCustomHandler);
        var client = factory.CreateDefaultClient(delegationgHandler);

        var registration = await client.PostAsJsonAsync("api/account/register", new
        {
            userName = "SuperTestUser",
            email = "Test@email.com",
            password = "Test_Pa$sw-0rd"
        });

        var loginResponse = await client.PostAsJsonAsync("/login", new
        {
            Email = "Test@email.com",
            Password = "Test_Pa$sw-0rd"
        });
        loginResponse.EnsureSuccessStatusCode();

        return client;
    }
}

internal class PassThroughHandler(HttpMessageHandler innerHandler) : DelegatingHandler(innerHandler)
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => base.SendAsync(request, cancellationToken);
}