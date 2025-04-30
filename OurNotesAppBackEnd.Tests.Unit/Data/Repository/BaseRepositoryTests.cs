using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Data;
using OurNotesAppBackEnd.Data.Repositories;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEndTests.Data.Repository;

public class BaseRepositoryTests
{
    private ApplicationDbContext? _context;
    private readonly Guid _noteId = Guid.NewGuid();
    private readonly Note _note;

    public BaseRepositoryTests()
    {
        _note = new Note()
        {
            Id = _noteId,
            Title = "Some note title",
            Content = "Some note important content",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
    }

    [Fact]
    public async void BaseRepository_GetAllEntitiesAsync_Return_All_Notes()
    {
        // Arrange
        var countOfNotes = 10;
        for (var i = 0; i < countOfNotes; i++)
        {
            _context!.Notes.Add(new Note()
            {
                Title = "Some note title",
                Content = "Some note important content",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        await _context!.SaveChangesAsync();
        var notesRepository = new BaseRepository<Note, Guid>(_context);

        // Act
        var result = (await notesRepository.GetAllEntitiesAsync()).ToList();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(countOfNotes);
        result.Should().OnlyContain(note => note != null);
        result.Should().BeInDescendingOrder(note => note.Id);
        result.Should().AllBeOfType<Note>();
        result.Select(note => note.Id).Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public async void BaseRepository_GetAllEntitiesAsync_Return_Empty_Array_When_DataBase_Is_Empty()
    {
        // Arrange
        var notesRepository = new BaseRepository<Note, Guid>(_context!);

        // Act
        var result = (await notesRepository.GetAllEntitiesAsync()).ToList();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async void BaseRepository_GetEntityByIdAsync_Return_Note_With_Requested_Id()
    {
        // Arrange
        await _context!.Notes.AddAsync(_note);
        await _context.SaveChangesAsync();
        var noteRepository = new BaseRepository<Note, Guid>(_context);

        // Act
        var result = await noteRepository.GetEntityByIdAsync(_noteId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Note>();
        result.Id.Should().Be(_noteId);
    }

    [Fact]
    public async void BaseRepository_AddEntityAsync_New_Note_Should_Be_Added_To_DataBase()
    {
        // Arrange
        var noteRepository = new BaseRepository<Note, Guid>(_context!);

        // Act
        await noteRepository.AddEntityAsync(_note);

        // Assert
        var newAddedNote = await noteRepository.GetEntityByIdAsync(_noteId);
        newAddedNote.Should().NotBeNull();
        newAddedNote.Should().BeOfType<Note>();
        newAddedNote.Should().Be(_note);
    }

    [Fact]
    public async void BaseRepository_EditEntity_Note_Should_Be_Changed_In_DataBase()
    {
        // Arrange
        await _context!.Notes.AddAsync(_note);
        await _context.SaveChangesAsync();

        _context.Entry(_note).State = EntityState.Detached;

        var updatedNote = new Note()
        {
            Id = _noteId,
            Title = "New title UPDATED",
            Content = "New Content UPDATED",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var noteRepository = new BaseRepository<Note, Guid>(_context!);

        // Act
        await noteRepository.EditEntity(updatedNote);

        // Assert
        var result = await noteRepository.GetEntityByIdAsync(_noteId);
        result.Should().NotBeNull();
        result.Should().BeOfType<Note>();
        result.Title.Should().Be("New title UPDATED");
        result.Content.Should().Be("New Content UPDATED");
    }

    [Fact]
    public async void BaseRepository_DeleteEntity_Should_Remove_Note_From_The_DataBase()
    {
        // Arrange
        await _context!.Notes.AddAsync(_note);
        await _context.SaveChangesAsync();
        var noteRepository = new BaseRepository<Note, Guid>(_context);

        // Act
        await noteRepository.DeleteEntity(_note);

        // Assert
        var requestedNote = await noteRepository.GetEntityByIdAsync(_noteId);
        requestedNote.Should().BeNull();
    }
}