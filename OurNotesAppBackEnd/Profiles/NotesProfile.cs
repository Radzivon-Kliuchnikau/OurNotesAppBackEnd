using AutoMapper;
using OurNotesAppBackEnd.Dtos;
using OurNotesAppBackEnd.Dtos.Note;
using OurNotesAppBackEnd.Dtos.User;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Profiles;

public class NotesProfile : Profile
{
    public NotesProfile()
    {
        // Source -> Target
        CreateMap<AppUser, AuthorReadDto>();
        CreateMap<Note, NoteReadDto>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.AppUser));
        CreateMap<NoteCreateDto, Note>()
            .ForMember(dest => dest.NoteAccesses, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.CreatedAt = DateTime.UtcNow;
                dest.UpdatedAt = DateTime.UtcNow;
            });
        CreateMap<NoteUpdateDto, Note>()
            .AfterMap((src, dest) => { dest.UpdatedAt = DateTime.UtcNow; });
        CreateMap<Note, NoteUpdateDto>();
    }
}