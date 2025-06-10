namespace RoundaboutBlog.Dto;

public class CommentViewDto
{
  public int CommentId { get; set; }

  public required string Title { get; set; }

  public required string Content { get; set; }

  public DateTime CreatedAt { get; set; }


  public required string AuthorId { get; set; }

  public string? Author { get; set; }
}