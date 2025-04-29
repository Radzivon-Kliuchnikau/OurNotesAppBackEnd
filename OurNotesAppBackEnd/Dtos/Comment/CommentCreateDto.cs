using System.ComponentModel.DataAnnotations;

namespace OurNotesAppBackEnd.Dtos.Comment;

public class CommentCreateDto
{
    [Required]
    [MinLength(4, ErrorMessage = "Comment title must be 4 characters or more")]
    [MaxLength(280, ErrorMessage = "Comment title should not be more the 280 characters")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(4, ErrorMessage = "Comment content must be 4 characters or more")]
    [MaxLength(280, ErrorMessage = "Comment content should not be more the 280 characters")]
    public string Content { get; set; } = string.Empty;
}