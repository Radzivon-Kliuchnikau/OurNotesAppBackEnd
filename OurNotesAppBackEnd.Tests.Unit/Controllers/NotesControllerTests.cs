using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OurNotesAppBackEnd.Controllers;
using OurNotesAppBackEnd.Dtos;
using OurNotesAppBackEnd.Dtos.Note;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;
using OurNotesAppBackEnd.Profiles;
using Xunit.Abstractions;

namespace OurNotesAppBackEndTests.Controllers;

public class NotesControllerTests
{
    private INoteRepository _noteRepository;
    private IMapper _mapper;
    private ILogger<NotesController> _logger;
    private NotesController _noteController;
    private readonly ITestOutputHelper _output;
    
    public NotesControllerTests(ITestOutputHelper output)
    {
        _output = output;
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile<NotesProfile>();
        });
        _noteRepository = A.Fake<INoteRepository>();
        _mapper = config.CreateMapper();
        _logger = A.Fake<ILogger<NotesController>>();
        _noteController = new NotesController(_noteRepository, _mapper, _logger);
    }
    
    [Fact]
    public async Task NotesController_GetAllNotes_Return_The_Correct_Number_Of_Notes()
    {
        //Arrange
        var notesCount = 10;
        var fakeNotes = A.CollectionOfDummy<Note>(notesCount).AsEnumerable();
        var expectedDtos = _mapper.Map<List<NoteReadDto>>(fakeNotes);
        
        A.CallTo(() => _noteRepository.GetAllEntitiesAsync()).Returns(Task.FromResult(fakeNotes));

        //Act
        var result = await _noteController.GetAllNotes();
        var okResult = result.Result as OkObjectResult;
        var returnedNotes = okResult?.Value as List<NoteReadDto>;

        //Assert
        okResult.Should().NotBeNull();
        returnedNotes.Should().NotBeNull();
        returnedNotes.Count().Should().Be(notesCount).And.Be(expectedDtos.Count);
        _output.WriteLine("Correct number of notes verified");
        
    }

    [Fact]
    public async Task NotesController_GetNoteById_Return_The_Correct_Note_By_Provided_Id()
    {
        //Arrange
        var idValue = new Guid("B144F85A-2D45-448E-8949-E585BE051F14");
        var fakeNote = A.Fake<Note>();
        fakeNote.Id = idValue;
        var expectedDtoNote = _mapper.Map<NoteReadDto>(fakeNote);
        
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(idValue)).Returns(Task.FromResult<Note?>(fakeNote));
        
        //Act
        var result = await _noteController.GetNoteById(idValue);
        var okResult = result.Result as OkObjectResult;
        var returnedNotes = okResult?.Value as NoteReadDto;

        //Assert
        result.Should().BeOfType<ActionResult<NoteReadDto>?>();
        okResult.Should().NotBeNull();
        returnedNotes.Should().NotBeNull();
        returnedNotes.Id.Should().Be(expectedDtoNote.Id);
    }

    [Fact]
    public async Task NotesController_GetNoteById_Return_The_Not_Found()
    {
        //Arrange
        var unknownId = new Guid();
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(unknownId)).Returns(Task.FromResult<Note?>(null));

        //Act
        var result = await _noteController.GetNoteById(unknownId);

        //Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task NotesController_CreateNote_Return_CreatedAtRoute_New_Note()
    {
        //Arrange
        var generatedNoteId = new Guid();
        var fakeNoteCreateDto = A.Fake<NoteCreateDto>();
        var expectedNoteDto = A.Fake<NoteReadDto>();
        expectedNoteDto.Id = generatedNoteId;

        A.CallTo(() => _noteRepository.AddEntityAsync(A<Note>.Ignored))
            .Invokes((Note n) => n.Id = generatedNoteId)
            .Returns(Task.CompletedTask);
        
        //Act
        var result = await _noteController.CreateNote(fakeNoteCreateDto);
        var createdAtResult = result.Result as CreatedAtRouteResult;
        var returnedNote = createdAtResult?.Value as NoteReadDto;

        //Assert
        createdAtResult.Should().NotBeNull();
        returnedNote.Should().NotBeNull();
        returnedNote.Id.Should().Be(expectedNoteDto.Id);
    }

    [Fact]
    public async Task NotesController_UpdateNote_Return_NoContent_After_Note_Updating()
    {
        //Arrange
        var updatedNoteId = new Guid();
        var fakeUpdateDto = A.Fake<NoteUpdateDto>();
        var fakeNote = A.Fake<Note>();
        fakeNote.Id = updatedNoteId;

        A.CallTo(() => _noteRepository.GetEntityByIdAsync(updatedNoteId)).Returns(Task.FromResult<Note?>(fakeNote));
        A.CallTo(() => _noteRepository.EditEntity(fakeNote)).DoesNothing();

        //Act
        var result = await _noteController.UpdateNote(updatedNoteId, fakeUpdateDto);
        
        //Assert
        result.Should().BeOfType<NoContentResult>();
        A.CallTo(() => _noteRepository.EditEntity(fakeNote)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task NotesController_UpdateNote_Return_NotFound_When_Non_Existing_Note_Id_Provided()
    {
        //Arrange
        var nonExistingNoteId = new Guid();
        var fakeUpdateDto = A.Fake<NoteUpdateDto>();
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(nonExistingNoteId)).Returns(Task.FromResult<Note?>(null));

        //Act
        var result = await _noteController.UpdateNote(nonExistingNoteId, fakeUpdateDto);
        
        //Assert
        result.Should().BeOfType<NotFoundResult>();
        A.CallTo(() => _noteRepository.EditEntity(A<Note>.Ignored)).MustNotHaveHappened();
    }

    [Fact]
    public async Task NotesController_DeleteNote_Return_NoContent_After_Note_Deleted()
    {
        //Arrange
        var deletedNoteId = new Guid();
        var fakeNote = A.Fake<Note>();
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(deletedNoteId)).Returns(Task.FromResult<Note?>(fakeNote));
        A.CallTo(() => _noteRepository.DeleteEntity(fakeNote)).DoesNothing();

        //Act
        var result = await _noteController.DeleteNote(deletedNoteId);

        //Assert
        result.Should().BeOfType<NoContentResult>();
        A.CallTo(() => _noteRepository.DeleteEntity(fakeNote)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task NotesController_DeleteNote_Return_NotFound_When_Non_Existing_Note_Id_Provided()
    {
        //Arrange
        var nonExistingId = new Guid();
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(nonExistingId)).Returns(Task.FromResult<Note?>(null));

        //Act
        var result = await _noteController.DeleteNote(nonExistingId);

        //Assert
        result.Should().BeOfType<NotFoundResult>();
        A.CallTo(() => _noteRepository.DeleteEntity(A<Note>.Ignored)).MustNotHaveHappened();
    }
}