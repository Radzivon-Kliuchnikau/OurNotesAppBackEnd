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
        CreateMap<NoteCreateDto, Note>()
            .AfterMap((src, dest) =>
            {
                dest.CreatedAt = DateTime.Now;
                dest.UpdatedAt = DateTime.Now;
            });
        CreateMap<NoteUpdateDto, Note>()
            .AfterMap((src, dest) =>
            {
                dest.UpdatedAt = DateTime.Now;
            });
        CreateMap<Note, NoteUpdateDto>();
    }
}