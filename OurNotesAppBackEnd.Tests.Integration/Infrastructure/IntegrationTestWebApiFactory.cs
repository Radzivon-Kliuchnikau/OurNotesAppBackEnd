using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OurNotesAppBackEnd.Data;
using Respawn;
using Testcontainers.SqlEdge;

namespace OurNotesAppBackEnd.Tests.Integration.Infrastructure;

public class IntegrationTestWebApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private DbConnection _dbConnection = default;
    private Respawner _respawner = default;
    
    private readonly SqlEdgeContainer _testingSqlDb = new SqlEdgeBuilder()
        .WithPassword("W_password$2")
        .Build();

    public HttpClient HttpClient = default!;
    
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
            services.AddDbContext<NotesAppDbContext>(options => options.UseSqlServer(_testingSqlDb.GetConnectionString(), sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null)));
        });
    }

    public async Task InitializeAsync()
    {
        await _testingSqlDb.StartAsync();
        HttpClient = CreateClient();
        // await InitializeDataBaseRespawn();
    }

    public async Task DisposeAsync()
    {
        await _testingSqlDb.StopAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
    
    private async Task InitializeDataBaseRespawn()
    {
        _dbConnection = new SqlConnection(_testingSqlDb.GetConnectionString());
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions()
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = new []{"dbo"}
        });
    }
}