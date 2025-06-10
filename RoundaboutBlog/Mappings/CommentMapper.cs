using RoundaboutBlog.Dto;
using RoundaboutBlog.Entities;

namespace RoundaboutBlog.Mappings;

public static class CommentMapper
{
  // CreateDto
  public static Comment ToComment(this CommentCreateDto? dto)
  {
    return new Comment()
    {
      Title = dto.Title,
      Content = dto.Content,
    };
  }

  // ViewDto
  public static CommentViewDto ToViewDto(this Comment comment)
  {
    return new CommentViewDto
    {
      CommentId = comment.CommentId,
      Title = comment.Title,
      Content = comment.Content,
      CreatedAt = comment.CreatedAt,
      AuthorId = comment.UserId!,
      Author = comment.User?.UserName ?? String.Empty
    };
  }

  public static Comment ToComment(this CommentViewDto dto)
  {
    return new Comment()
    {
      CommentId = dto.CommentId,
      Title = dto.Title,
      Content = dto.Content,
      CreatedAt = dto.CreatedAt,
      UserId = dto.AuthorId
    };
  }
}