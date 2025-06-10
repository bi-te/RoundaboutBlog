namespace RoundaboutBlog.Dto;

public class PostViewDto
{
  public int PostId { get; set; }
  public required string Title { get; set; }
  public required string Content { get; set; }
  public DateTime CreatedAt { get; set; }


  public required string AuthorId { get; set; }

  public string? AuthorName { get; set; }
}