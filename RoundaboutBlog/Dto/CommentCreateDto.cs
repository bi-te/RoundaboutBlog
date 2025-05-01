using System.ComponentModel.DataAnnotations;

namespace RoundaboutBlog.Dto;

public class CommentCreateDto
{
    [Required]
    [StringLength(200)]
    public required string Title { get; set; }

    [Required]
    [StringLength(2000)]
    public required string Content { get; set; }
}