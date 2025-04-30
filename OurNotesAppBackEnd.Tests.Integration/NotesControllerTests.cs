using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using OurNotesAppBackEnd.Dtos;
using OurNotesAppBackEnd.Dtos.Note;
using OurNotesAppBackEnd.Tests.Integration.Infrastructure;

namespace OurNotesAppBackEnd.Tests.Integration;

public class NotesControllerTests(IntegrationTestWebApiFactory webApiFactory)
    : IClassFixture<IntegrationTestWebApiFactory>
{
    private readonly HttpClient _httpTestClient = webApiFactory.TestClient;

    [Fact]
    public async Task NotesController_GetAllNotes_Return_Unauthorized_When_User_Not_Logged_In()
    {
        // Arrange
        // Act
        var response = await _httpTestClient.GetAsync("api/notes");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task NotesController_GetAllNotes_Return_All_Notes_From_Database()
    {
        // Arrange
        // Act
        var response = await _httpTestClient.GetAsync("api/notes");
        var result = response.Content.ReadFromJsonAsync<NoteReadDto>(); 

        // Assert
        // response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
    }
}