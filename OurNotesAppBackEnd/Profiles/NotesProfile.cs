using AutoMapper;
using OurNotesAppBackEnd.Dtos;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Profiles;

public class NotesProfile : Profile
{
    public NotesProfile()
    {
        // Source -> Target
        CreateMap<Note, NoteReadDto>();
        CreateMap<NoteCreateDto, Note>();
        CreateMap<NoteUpdateDto, Note>();
        CreateMap<Note, NoteUpdateDto>();
    }
}