namespace RoundaboutBlog.Dto;

public class PostViewDto
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    
    
    public string AuthorId { get; set; }
    
    public string AuthorName { get; set; }
}