using AutoMapper;
using OurNotesAppBackEnd.Dtos;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Profiles;

public class NotesProfile : Profile
{
    public NotesProfile()
    {
        CreateMap<Note, NoteReadDto>();
    }
}