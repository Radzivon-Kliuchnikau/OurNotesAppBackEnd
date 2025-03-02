using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using OurNotesAppBackEnd.Controllers;
using OurNotesAppBackEnd.Dtos;
using OurNotesAppBackEnd.Models;
using OurNotesAppBackEnd.Profiles;
using OurNotesAppBackEnd.Services;

namespace OurNotesAppBackEndTests.Controllers;

public class NotesControllerTests
{
    private INotesService _noteService;
    private IMapper _mapper;
    private NotesController _noteController;
    
    public NotesControllerTests()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile<NotesProfile>();
        });
        _noteService = A.Fake<INotesService>();
        _mapper = config.CreateMapper();
        _noteController = new NotesController(_noteService, _mapper);
    }
    
    [Fact]
    public void GetAllNotes_Return_The_Correct_Number_Of_Notes()
    {
        //Arrange
        var notesCount = 10;
        var fakeNotes = A.CollectionOfDummy<Note>(notesCount).AsEnumerable();
        var expectedDtos = _mapper.Map<List<NoteReadDto>>(fakeNotes);
        
        A.CallTo(() => _noteService.GetAllNotes()).Returns(fakeNotes);

        //Act
        var result = _noteController.GetAllNotes();
        var okResult = result.Result as OkObjectResult;
        var returnedNotes = okResult.Value as List<NoteReadDto>;

        //Assert
        okResult.Should().NotBeNull();
        returnedNotes.Should().NotBeNull();
        returnedNotes.Count().Should().Be(notesCount).And.Be(expectedDtos.Count);
        
    }

    [Fact]
    public void GetNoteById_Return_The_Correct_Note_By_Provided_Id()
    {
        //Arrange
        var idValue = "B144F85A-2D45-448E-8949-E585BE051F14";
        var fakeNote = A.Fake<Note>();
        fakeNote.Id = idValue;
        var expectedDtoNote = _mapper.Map<NoteReadDto>(fakeNote);
        
        A.CallTo(() => _noteService.GetNoteById(idValue)).Returns(fakeNote);
        
        //Act
        var result = _noteController.GetNoteById(idValue);
        var okResult = result.Result as OkObjectResult;
        var returnedNotes = okResult.Value as NoteReadDto;

        //Assert
        okResult.Should().NotBeNull();
        returnedNotes.Should().NotBeNull();
        returnedNotes.Id.Should().Be(expectedDtoNote.Id);
    }

    [Fact]
    public void GetNoteById_Return_The_Not_Found()
    {
        //Arrange
        var unknownId = "fakeId";
        A.CallTo(() => _noteService.GetNoteById(unknownId)).Returns(null);

        //Act
        var result = _noteController.GetNoteById(unknownId);

        //Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void CreateNote_Return_CreatedAtRoute_New_Note()
    {
        //Arrange
        var generatedNoteId = "generated_note_id";
        var fakeNoteCreateDto = A.Fake<NoteCreateDto>();
        var expectedNoteDto = A.Fake<NoteReadDto>();
        expectedNoteDto.Id = generatedNoteId;

        A.CallTo(() => _noteService.AddNote(A<Note>.Ignored)).Invokes((Note n) => n.Id = generatedNoteId);
        
        //Act
        var result = _noteController.CreateNote(fakeNoteCreateDto);
        var createdAtResult = result.Result as CreatedAtRouteResult;
        var returnedNote = createdAtResult?.Value as NoteReadDto;

        //Assert
        createdAtResult.Should().NotBeNull();
        returnedNote.Should().NotBeNull();
        returnedNote.Id.Should().Be(expectedNoteDto.Id);
    }

    [Fact]
    public void UpdateNote_Return_NoContent_After_Note_Updating()
    {
        //Arrange
        var updatedNoteId = "updated_note_id";
        var fakeUpdateDto = A.Fake<NoteUpdateDto>();
        var fakeNote = A.Fake<Note>();
        fakeNote.Id = updatedNoteId;

        A.CallTo(() => _noteService.GetNoteById(updatedNoteId)).Returns(fakeNote);
        A.CallTo(() => _noteService.EditNote(fakeNote)).DoesNothing();

        //Act
        var result = _noteController.UpdateNote(updatedNoteId, fakeUpdateDto);
        
        //Assert
        result.Should().BeOfType<NoContentResult>();
        A.CallTo(() => _noteService.EditNote(fakeNote)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void UpdateNote_Return_NotFound_When_Non_Existing_Note_Id_Provided()
    {
        //Arrange
        var nonExistingNoteId = "non_existing_note_id";
        var fakeUpdateDto = A.Fake<NoteUpdateDto>();
        A.CallTo(() => _noteService.GetNoteById(nonExistingNoteId)).Returns(null);

        //Act
        var result = _noteController.UpdateNote(nonExistingNoteId, fakeUpdateDto);
        
        //Assert
        result.Should().BeOfType<NotFoundResult>();
        A.CallTo(() => _noteService.EditNote(A<Note>.Ignored)).MustNotHaveHappened();
    }

    [Fact]
    public void DeleteNote_Return_NoContent_After_Note_Deleted()
    {
        //Arrange
        var deletedNoteId = "deleted_note_id";
        var fakeNote = A.Fake<Note>();
        A.CallTo(() => _noteService.GetNoteById(deletedNoteId)).Returns(fakeNote);
        A.CallTo(() => _noteService.DeleteNote(fakeNote)).DoesNothing();

        //Act
        var result = _noteController.DeleteNote(deletedNoteId);

        //Assert
        result.Should().BeOfType<NoContentResult>();
        A.CallTo(() => _noteService.DeleteNote(fakeNote)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void DeleteNote_Return_NotFound_When_Non_Existing_Note_Id_Provided()
    {
        //Arrange
        var nonExistingId = "non_existing_id";
        A.CallTo(() => _noteService.GetNoteById(nonExistingId)).Returns(null);

        //Act
        var result = _noteController.DeleteNote(nonExistingId);

        //Assert
        result.Should().BeOfType<NotFoundResult>();
        A.CallTo(() => _noteService.DeleteNote(A<Note>.Ignored)).MustNotHaveHappened();
    }
}