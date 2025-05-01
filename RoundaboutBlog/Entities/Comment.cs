using System.ComponentModel.DataAnnotations;

namespace RoundaboutBlog.Entities;

public class Comment
{
    public int CommentId { get; set; }
    
    [StringLength(200)]
    public required string Title { get; set; }
    
    [StringLength(2000)]
    public required string Content { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    
    public string? UserId { get; set; }
    
    public AppUser? User { get; set; }
    
    public int PostId { get; set; }
    
    public Post? Post { get; set; }
    
}