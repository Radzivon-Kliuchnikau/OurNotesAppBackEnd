using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using OurNotesAppBackEnd.Dtos;
using OurNotesAppBackEnd.Tests.Integration.Infrastructure;

namespace OurNotesAppBackEnd.Tests.Integration;

public class NotesControllerIntegrationTests : IClassFixture<IntegrationTestWebApiFactory>, IAsyncLifetime
{
    private readonly HttpClient _httpСlient;
    // private Func<Task> _resetDatabase;
    
    public NotesControllerIntegrationTests(IntegrationTestWebApiFactory integrationTestWebApplicationFactory)
    {
        _httpСlient = integrationTestWebApplicationFactory.HttpClient;
        // _resetDatabase = integrationTestWebApplicationFactory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task NotesController_GetAllNotes_Return_All_Notes_From_Database()
    {
        // Arrange
        // Act
        var response = await _httpСlient.GetAsync("api/notes");
        var result = response.Content.ReadFromJsonAsync<NoteReadDto>(); 

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        // await _resetDatabase();
    }
}