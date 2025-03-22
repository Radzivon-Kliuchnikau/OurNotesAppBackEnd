using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Data;
using OurNotesAppBackEnd.Data.Repository;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEndTests.Data.Repository;

public class BaseRepositoryTests : IAsyncLifetime
{
    private const int AmountOfNotes = 10;
    private BaseRepository<Note, string>? _repository;
    private NotesAppDbContext? _context;

    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<NotesAppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new NotesAppDbContext(options);
        await _context.Database.EnsureCreatedAsync();
        for (var i = 0; i < AmountOfNotes; i++)
        {
            _context.Notes.Add(
                new Note()
                {
                    Title = $"Some note title {i}",
                    Content = $"Some note important content {i}",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
        }
        await _context.SaveChangesAsync();
        _repository = new BaseRepository<Note, string>(_context);
    }

    public Task DisposeAsync()
    {
        _context!.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async void BaseRepository_GetAllEntitiesAsync_Return_All_Notes()
    {
        // Act
        var result = (await _repository!.GetAllEntitiesAsync()).ToList();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(AmountOfNotes);
        result.Should().OnlyContain(note => note != null);
        result.Should().BeInDescendingOrder(note => note.Id);
        result.Should().AllBeOfType<Note>();
        result.Select(note => note.Id).Should().OnlyHaveUniqueItems();
    }
}