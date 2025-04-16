using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OurNotesAppBackEnd.Data;
using OurNotesAppBackEnd.Identity;
using Testcontainers.SqlEdge;

namespace OurNotesAppBackEnd.Tests.Integration.Infrastructure;

public class IntegrationTestWebApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    public HttpClient TestClient = default!;
    
    private readonly SqlEdgeContainer _testingSqlDb = new SqlEdgeBuilder()
        .WithPassword("W_password$2")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            var identityDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<ApplicationIdentityDbContext>));
            if (descriptor != null && identityDescriptor != null)
            {
                services.Remove(descriptor);
                services.Remove(identityDescriptor);
            }

            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(_testingSqlDb.GetConnectionString(),
                    sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null)));
            services.AddDbContext<ApplicationIdentityDbContext>(options =>
                options.UseSqlServer(_testingSqlDb.GetConnectionString(),
                    sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null)));
        });
    }

    public async Task InitializeAsync()
    {
        await _testingSqlDb.StartAsync();
        TestClient = CreateClient();
    }

    public async Task DisposeAsync()
    {
        await _testingSqlDb.StopAsync();
    }
}