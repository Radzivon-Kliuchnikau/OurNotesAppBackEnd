using System.Security.Claims;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OurNotesAppBackEnd.Controllers;
using OurNotesAppBackEnd.Dtos.Note;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;
using OurNotesAppBackEnd.Utils;
using Xunit.Abstractions;

namespace OurNotesAppBackEndTests.Controllers;

public class NotesControllerTests
{
    private INoteRepository _noteRepository;
    private IMapper _mapper;
    private ILogger<NotesController> _logger;
    private NotesController _noteController;
    private IGrantAccessToNoteService _grantAccessToNoteService;
    private readonly ITestOutputHelper _output;
    private ClaimsPrincipal _user;
    private readonly string _fakeUserId = Guid.NewGuid().ToString();
    
    public NotesControllerTests(ITestOutputHelper output)
    {
        _output = output;
        _noteRepository = A.Fake<INoteRepository>();
        _mapper = A.Fake<IMapper>();
        _logger = A.Fake<ILogger<NotesController>>();
        _grantAccessToNoteService = A.Fake<IGrantAccessToNoteService>();
        _noteController = new NotesController(_noteRepository, _mapper, _logger, _grantAccessToNoteService);
        _user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _fakeUserId)
        }, "mock"));
        _noteController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = _user }
        };
    }
    
    [Fact(DisplayName = "GetAllNotes returns Ok with the correct number of notes")]
    public async Task NotesController_GetAllNotes_Return_The_Correct_Number_Of_Notes()
    {
        //Arrange
        var notesCount = 10;
        var fakeNoteModelCollection = A.CollectionOfDummy<Note>(notesCount).AsEnumerable();
        var fakeNoteReadDtoCollection = A.CollectionOfDummy<NoteReadDto>(notesCount);
        
        A.CallTo(() => _noteRepository.GetNotesUserHaveAccessTo(_fakeUserId)).Returns(Task.FromResult(fakeNoteModelCollection));
        A.CallTo(() => _mapper.Map<IEnumerable<NoteReadDto>>(fakeNoteModelCollection)).Returns(fakeNoteReadDtoCollection);

        //Act
        var result = await _noteController.GetAllNotes();
        var okResult = result.Result as OkObjectResult;
        var returnedNotes = okResult?.Value as List<NoteReadDto>;

        //Assert
        okResult.Should().NotBeNull();
        returnedNotes.Should().NotBeNull();
        returnedNotes.Count.Should().Be(notesCount).And.Be(fakeNoteReadDtoCollection.Count);
        A.CallTo(() => _noteRepository.GetNotesUserHaveAccessTo(_fakeUserId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mapper.Map<IEnumerable<NoteReadDto>>(fakeNoteModelCollection)).MustHaveHappenedOnceExactly();
        _output.WriteLine("Correct number of NoteReadDtos returned successfully");
        
    }
    
    [Fact(DisplayName = "GetAllNotes returns Unauthorized when UserId is null")]
    public async Task NotesController_GetAllNotes_Return_Unauthorized_When_UserId_Equals_Null()
    {
        //Arrange
        _noteController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
        };

        //Act
        var result = await _noteController.GetAllNotes();
        var unauthorizedResult = result.Result as UnauthorizedObjectResult;

        //Assert
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        unauthorizedResult.Value.Should().Be(ResultMessages.UserIdNotFound);
        A.CallTo(() => _noteRepository.GetNotesUserHaveAccessTo(A<string>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => _mapper.Map<IEnumerable<NoteReadDto>>(A<IEnumerable<Note>>.Ignored)).MustNotHaveHappened();
        _output.WriteLine("Endpoint returned UnauthorizedObjectResult as expected when UserId is null");
        
    }

    [Fact(DisplayName = "GetNoteById returns Ok with the correct note when a valid ID is provided")]
    public async Task NotesController_GetNoteById_Return_The_Correct_Note_By_Provided_Id()
    {
        //Arrange
        var requestedNoteId = Guid.NewGuid();
        var fakeNoteReadDto = A.Fake<NoteReadDto>();
        
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(requestedNoteId)).Returns(Task.FromResult<Note?>(A.Fake<Note>()));
        A.CallTo(() => _mapper.Map<NoteReadDto>(A<Note>.Ignored)).Returns(fakeNoteReadDto);
        
        //Act
        var result = await _noteController.GetNoteById(requestedNoteId);
        var okResult = result.Result as OkObjectResult;
        var returnedNoteReadDto = okResult?.Value as NoteReadDto;

        //Assert
        result.Should().BeOfType<ActionResult<NoteReadDto>?>();
        okResult.Should().NotBeNull();
        returnedNoteReadDto.Should().NotBeNull();
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(requestedNoteId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mapper.Map<NoteReadDto>(A<Note>.Ignored)).MustHaveHappenedOnceExactly();
        _output.WriteLine("NoteReadDto retrieved successfully by ID");
    }

    [Fact(DisplayName = "GetNoteById return NotFound when an unknown ID is used")]
    public async Task NotesController_GetNoteById_Return_The_Not_Found_When_UnknownId_Used()
    {
        //Arrange
        var unknownNoteId = Guid.NewGuid();
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(unknownNoteId)).Returns(Task.FromResult<Note?>(null));

        //Act
        var result = await _noteController.GetNoteById(unknownNoteId);

        //Assert
        result.Result.Should().BeOfType<NotFoundResult>();
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(unknownNoteId)).MustHaveHappenedOnceExactly();
        _output.WriteLine("NotFoundResult returned as expected for unknown ID");
    }

    [Fact(DisplayName = "CreateNote returns a new Note through CreatedAtRoute")]
    public async Task NotesController_CreateNote_Returns_CreatedAtRoute_When_Valid_Request()
    {
        //Arrange
        A.CallTo(() => _mapper.Map<Note>(A<NoteCreateDto>.Ignored)).Returns(A.Fake<Note>());
        A.CallTo(() => _mapper.Map<NoteReadDto>(A<Note>.Ignored)).Returns(A.Fake<NoteReadDto>());
        A.CallTo(() => _noteRepository.AddEntityAsync(A<Note>.Ignored)).Returns(Task.CompletedTask);
        A.CallTo(() => _grantAccessToNoteService.GrantAccessToNoteAsync(A<string>.Ignored, A<Note>.Ignored, A<IEnumerable<string>>.Ignored))
            .Returns(Task.CompletedTask);
        
        //Act
        var result = await _noteController.CreateNote(A.Fake<NoteCreateDto>());
        var createdAtResult = result.Result as CreatedAtRouteResult;
        var returnedNote = createdAtResult?.Value as NoteReadDto;

        //Assert
        createdAtResult.Should().NotBeNull();
        createdAtResult.RouteName.Should().Be(nameof(_noteController.GetNoteById));
        returnedNote.Should().NotBeNull();
        A.CallTo(() => _noteRepository.AddEntityAsync(A<Note>.That.Matches(n => n.AppUserId == _fakeUserId))).MustHaveHappenedOnceExactly();
        A.CallTo(() => _grantAccessToNoteService.GrantAccessToNoteAsync(A<string>.Ignored, A<Note>.Ignored, A<IEnumerable<string>>.Ignored))
            .MustHaveHappenedOnceExactly();
        _output.WriteLine("Note created successfully and returned through CreatedAtRoute");
    }
    
    [Fact(DisplayName = "CreateNote returns Unauthorized when UserId is null")]
    public async Task NotesController_CreateNote_Returns_Unauthorized_When_UserID_Is_Null()
    {
        //Arrange
        _noteController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
        };
        
        //Act
        var result = await _noteController.CreateNote(A.Fake<NoteCreateDto>());
        var unauthorizedResult = result.Result as UnauthorizedObjectResult;

        //Assert
        unauthorizedResult.Should().NotBeNull();
        A.CallTo(() => _noteRepository.AddEntityAsync(A<Note>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => _grantAccessToNoteService.GrantAccessToNoteAsync(A<string>.Ignored, A<Note>.Ignored, A<IEnumerable<string>>.Ignored))
            .MustNotHaveHappened();
        _output.WriteLine("UnauthorizedObjectResult returned as expected when UserId is null");
    }
    
    [Fact(DisplayName = "GrantAccessToNote returns Ok when access is granted successfully")]
    public async Task NotesController_GrantAccessToNote_Returns_Ok_When_Access_Granted_Successfully()
    {
        //Arrange
        var noteId = Guid.NewGuid();
        var fakeNoteModel = A.Fake<Note>();
        
        //Act
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(noteId)).Returns(Task.FromResult<Note?>(fakeNoteModel));
        A.CallTo(() => _grantAccessToNoteService.GrantAccessToNoteAsync(A<string>.Ignored, fakeNoteModel, A<IEnumerable<string>>.Ignored))
            .DoesNothing();
        var result = await _noteController.GrantAccessToNote(noteId.ToString(), ["example@example.com"]);

        //Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult?.Value.Should().Be(ResultMessages.AccessGrantedSuccessfully);
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(noteId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _grantAccessToNoteService.GrantAccessToNoteAsync(A<string>.Ignored, fakeNoteModel, A<IEnumerable<string>>.Ignored))
            .MustHaveHappenedOnceExactly();
        _output.WriteLine("GrantAccessToNote called with correct parameters");
    }
    
    [Fact(DisplayName = "GrantAccessToNote returns NotFound when Note does not exist")]
    public async Task NotesController_GrantAccessToNote_Returns_NotFound_When_Note_Does_Not_Exist()
    {
        //Arrange
        var noteId = Guid.NewGuid();
        
        //Act
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(noteId)).Returns(Task.FromResult<Note?>(null));
        var result = await _noteController.GrantAccessToNote(noteId.ToString(), ["example@example.com"]);

        //Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult?.Value.Should().Be(ResultMessages.NoteNotFound);
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(noteId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _grantAccessToNoteService.GrantAccessToNoteAsync(A<string>.Ignored, A<Note>.Ignored, A<IEnumerable<string>>.Ignored)).MustNotHaveHappened();
        _output.WriteLine("NotFoundObjectResult returned as expected when Note does not exist");
    }
    
    [Fact(DisplayName = "RemoveGrantedAccessToNote returns Ok when granted access removed successfully")]
    public async Task NotesController_RemoveGrantedAccessToNote_Returns_Ok_When_Granted_Access_Removed_Successfully()
    {
        //Arrange
        var noteId = Guid.NewGuid();
        var fakeNoteModel = A.Fake<Note>();
        
        //Act
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(noteId)).Returns(Task.FromResult<Note?>(fakeNoteModel));
        A.CallTo(() => _grantAccessToNoteService.RemoveGrantedAccessFromNoteAsync(A<string>.Ignored, fakeNoteModel, A<IEnumerable<string>>.Ignored))
            .DoesNothing();
        var result = await _noteController.RemoveGrantedAccessToNote(noteId.ToString(), ["example@example.com"]);

        //Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult?.Value.Should().Be(ResultMessages.AccessRemovedSuccessfully);
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(noteId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _grantAccessToNoteService.RemoveGrantedAccessFromNoteAsync(A<string>.Ignored, fakeNoteModel, A<IEnumerable<string>>.Ignored))
            .MustHaveHappenedOnceExactly();
        _output.WriteLine("Access to Note removed successfully and returned OkObjectResult");
    }
    
    [Fact(DisplayName = "RemoveGrantedAccessToNote returns Not Found when note does not exist")]
    public async Task NotesController_RemoveGrantedAccessToNote_Returns_NotFound_When_Note_Does_Not_Exist()
    {
        //Arrange
        var noteId = Guid.NewGuid();
        
        //Act
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(noteId)).Returns(Task.FromResult<Note?>(null));
        var result = await _noteController.RemoveGrantedAccessToNote(noteId.ToString(), ["example@example.com"]);

        //Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var okResult = result as NotFoundObjectResult;
        okResult?.Value.Should().Be(ResultMessages.NoteNotFound);
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(noteId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _grantAccessToNoteService.RemoveGrantedAccessFromNoteAsync(A<string>.Ignored, A<Note>.Ignored, A<IEnumerable<string>>.Ignored))
            .MustNotHaveHappened();
        _output.WriteLine("NotFoundObjectResult returned as expected when Note does not exist");
    }

    [Fact(DisplayName = "UpdateNote returns NoteReadDto after updating an existing note")]
    public async Task NotesController_UpdateNote_Return_NoteReadDto_After_Note_Updating()
    {
        //Arrange
        var updatedNoteId = Guid.NewGuid();
        var fakeNoteUpdateDto = A.Fake<NoteUpdateDto>();
        var fakeNoteModel = A.Fake<Note>();
        var fakeNoteReadDto = A.Fake<NoteReadDto>();

        A.CallTo(() => _noteRepository.GetEntityByIdAsync(updatedNoteId)).Returns(Task.FromResult<Note?>(fakeNoteModel));
        A.CallTo(() => _mapper.Map(fakeNoteUpdateDto, fakeNoteModel)).Invokes(() => {});
        A.CallTo(() => _mapper.Map<NoteReadDto>(fakeNoteModel)).Returns(fakeNoteReadDto);
        A.CallTo(() => _noteRepository.EditEntity(fakeNoteModel)).Returns(Task.CompletedTask);
        
        //Act
        var result = await _noteController.UpdateNote(updatedNoteId, fakeNoteUpdateDto);
        var okResult = result.Result as OkObjectResult;
        var returnedNote = okResult?.Value as NoteReadDto;
        
        //Assert
        result.Should().BeOfType<ActionResult<NoteReadDto>?>();
        okResult.Should().NotBeNull();
        returnedNote.Should().NotBeNull();
        A.CallTo(() => _noteRepository.EditEntity(fakeNoteModel)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(updatedNoteId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mapper.Map<NoteReadDto>(fakeNoteModel)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mapper.Map(fakeNoteUpdateDto, fakeNoteModel)).MustHaveHappenedOnceExactly();
        _output.WriteLine("NoteReadDto returned successfully after updating the note");
    }

    [Fact(DisplayName = "UpdateNote returns NotFound when a non-existing note ID is provided")]
    public async Task NotesController_UpdateNote_Return_NotFound_When_Non_Existing_Note_Id_Provided()
    {
        //Arrange
        var nonExistingNoteId = Guid.NewGuid();
        var fakeUpdateDto = A.Fake<NoteUpdateDto>();
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(nonExistingNoteId)).Returns(Task.FromResult<Note?>(null));

        //Act
        var result = await _noteController.UpdateNote(nonExistingNoteId, fakeUpdateDto);
        
        //Assert
        result.Result.Should().BeOfType<NotFoundResult>();
        A.CallTo(() => _noteRepository.EditEntity(A<Note>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(nonExistingNoteId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mapper.Map<NoteReadDto>(A<Note>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => _mapper.Map(fakeUpdateDto, A<Note>.Ignored)).MustNotHaveHappened();
        _output.WriteLine("NotFoundResult returned as expected for non-existing note ID");
    }

    [Fact(DisplayName = "DeleteNote returns NoContent after successfully deleting a note")]
    public async Task NotesController_DeleteNote_Return_NoContent_After_Note_Deleted()
    {
        //Arrange
        var deletedNoteId = Guid.NewGuid();
        var fakeNote = A.Fake<Note>();
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(deletedNoteId)).Returns(Task.FromResult<Note?>(fakeNote));
        A.CallTo(() => _noteRepository.DeleteEntity(fakeNote)).DoesNothing();

        //Act
        var result = await _noteController.DeleteNote(deletedNoteId);

        //Assert
        result.Should().BeOfType<NoContentResult>();
        A.CallTo(() => _noteRepository.DeleteEntity(fakeNote)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(deletedNoteId)).MustHaveHappenedOnceExactly();
        _output.WriteLine("NoContentResult returned successfully after deleting the note");
    }

    [Fact(DisplayName = "DeleteNote returns NotFound when a non-existing note ID is provided")]
    public async Task NotesController_DeleteNote_Return_NotFound_When_Non_Existing_Note_Id_Provided()
    {
        //Arrange
        var nonExistingId = Guid.NewGuid();
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(nonExistingId)).Returns(Task.FromResult<Note?>(null));

        //Act
        var result = await _noteController.DeleteNote(nonExistingId);

        //Assert
        result.Should().BeOfType<NotFoundResult>();
        A.CallTo(() => _noteRepository.DeleteEntity(A<Note>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => _noteRepository.GetEntityByIdAsync(nonExistingId)).MustHaveHappenedOnceExactly();
        _output.WriteLine("NotFoundResult returned as expected for non-existing note ID");
    }
}