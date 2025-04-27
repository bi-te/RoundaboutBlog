using RoundaboutBlog.Dto;
using RoundaboutBlog.Entities;

namespace RoundaboutBlog.Mappings;

public static class PostMapper
{
    public static Post ToPost(this PostCreateDto dto)
    {
        return new Post{ Title = dto.Title, Content = dto.Content };
    }
    
    //ViewDto
    public static PostViewDto ToViewDto(this Post post)
    {
        return new PostViewDto
        {
            PostId = post.PostId, 
            Title = post.Title, 
            Content = post.Content, 
            CreatedAt = post.CreatedAt,
            AuthorId = post.UserId,
            AuthorName = post.User?.UserName ?? ""
        };
    }
}