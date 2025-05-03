using AutoMapper;
using OurNotesAppBackEnd.Dtos.Comment;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Profiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        // Source -> Target
        CreateMap<Comment, CommentReadDto>();
        CreateMap<CommentCreateDto, Comment>()
            .AfterMap((src, dest) =>
            {
                dest.CreatedOn = DateTime.UtcNow;
            });
        CreateMap<CommentUpdateDto, Comment>();
    }
}