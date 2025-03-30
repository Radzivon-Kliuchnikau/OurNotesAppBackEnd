using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OurNotesAppBackEnd.Data;

namespace OurNotesAppBackEnd.Tests.Integration;

public class WebApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly IContainer _testingSqlDb = new ContainerBuilder()
        .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
        .WithName("testingSqlDb")
        .WithPortBinding(1500, 1433)
        .WithEnvironment("MSSQL_SA_PASSWORD", "K510_dbpass!")
        .WithEnvironment("ACCEPT_EULA", "Y")
        .WithEnvironment("MSSQL_PID", "Developer")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<NotesAppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var connectionString = $"Server={_testingSqlDb.Hostname},{_testingSqlDb.GetMappedPublicPort(1433)};Database=ournotes_tests;User Id=sa;Password=K510_dbpass!;encrypt=false;";

            services.AddDbContext<NotesAppDbContext>(options => options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null)));

        });
    }

    public async Task InitializeAsync()
    {
        await _testingSqlDb.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _testingSqlDb.StopAsync();
    }
}