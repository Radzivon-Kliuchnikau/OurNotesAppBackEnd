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
        CreateMap<CommentCreateDto, Comment>();
        CreateMap<CommentUpdateDto, Comment>();
    }
}