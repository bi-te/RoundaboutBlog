using System.ComponentModel.DataAnnotations;

namespace RoundaboutBlog.Dto;

public class PostCreateDto
{
    [Required(ErrorMessage = "Title required")]
    [StringLength(200)]
    public required string Title { get; set; }
    
    [Required(ErrorMessage = "Can't add empty post")]
    [StringLength(5000)]
    public required string Content { get; set; }
}