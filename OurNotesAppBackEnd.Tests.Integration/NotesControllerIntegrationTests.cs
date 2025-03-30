using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using OurNotesAppBackEnd.Dtos;

namespace OurNotesAppBackEnd.Tests.Integration;

public class NotesControllerIntegrationTests : IClassFixture<WebApiFactory>
{
    private readonly WebApiFactory _webApplicationFactory;

    public NotesControllerIntegrationTests(WebApiFactory webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory;
    }

    [Fact]
    public async Task NotesController_GetAllNotes_Return_All_Notes_From_Database()
    {
        // Arrange
        var httpClient = _webApplicationFactory.CreateClient();

        // Act
        var response = await httpClient.GetAsync("api/notes");
        var result = response.Content.ReadFromJsonAsync<NoteReadDto>(); 

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
    }
}