using System.Net;

namespace OurNotesAppBackEnd.Tests.Integration.Infrastructure;

public class BaseIntegrationTest : IAsyncLifetime
{
    protected readonly IntegrationTestWebApiFactory Factory;
    protected readonly HttpClient Client;
    private readonly CookieContainer _cookieContainer = new();

    public BaseIntegrationTest(IntegrationTestWebApiFactory factory)
    {
        Factory = factory;
        var handler = new HttpClientHandler() { CookieContainer = _cookieContainer };
        Client = Factory.CreateDefaultClient();
    }
    
    public Task InitializeAsync()
    {
        throw new NotImplementedException();
    }

    public Task DisposeAsync()
    {
        throw new NotImplementedException();
    }
}