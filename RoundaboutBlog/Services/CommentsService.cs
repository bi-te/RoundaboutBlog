using Microsoft.EntityFrameworkCore;
using RoundaboutBlog.Data;
using RoundaboutBlog.Dto;
using RoundaboutBlog.Entities;
using RoundaboutBlog.Mappings;

namespace RoundaboutBlog.Services;

public interface ICommentsService
{
    Task<Comment?> GetCommentEntityAsync(int id);
    Task<CommentViewDto?> GetCommentAsync(int id);
    Task<ICollection<CommentViewDto>> GetPostCommentsAsync(int postId);
    Task<CommentViewDto?> AddComment(string userId, int postId, CommentCreateDto dto);
    Task UpdateCommentAsync(int id, CommentCreateDto updateDto);
    Task DeleteCommentAsync(int id);
}

public class CommentsService: ICommentsService
{
    private readonly AppDbContext _dbContext;

    public CommentsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Comment?> GetCommentEntityAsync(int id)
    {
        return await _dbContext.Comments.Where(c => c.CommentId == id)
                                        .Include(c => c.Post)
                                        .Include(c => c.User)
                                        .SingleOrDefaultAsync();
    }
    
    public async Task<CommentViewDto?> GetCommentAsync(int id)
    {
        return (await GetCommentEntityAsync(id))?.ToViewDto();
    }

    public async Task<ICollection<CommentViewDto>> GetPostCommentsAsync(int postId)
    {
        return await _dbContext.Comments.Where(c => c.PostId == postId)
                                        .OrderByDescending(c => c.CreatedAt)
                                        .Include(c => c.User)
                                        .Select(c => c.ToViewDto())
                                        .ToListAsync();
    }

    public async Task<CommentViewDto?> AddComment(string userId, int postId, CommentCreateDto dto)
    {
        AppUser? user = await _dbContext.Users.FindAsync(userId);
        if (user is null)
        {
            return null;
        }

        Post? post = await _dbContext.Posts.FindAsync(postId);
        if (post is null)
        {
            return null;
        }

        Comment newComment = dto.ToComment();
        newComment.UserId = user.Id;
        newComment.PostId = post.PostId;

        var comment = await _dbContext.AddAsync(newComment);
        await _dbContext.SaveChangesAsync();

        comment.Entity.User = user;
        comment.Entity.Post = post;
        return comment.Entity.ToViewDto();
    }
    
    public async Task UpdateCommentAsync(int id, CommentCreateDto updateDto)
    {
        Comment? existing = await _dbContext.Comments.FindAsync(id);
        
        if (existing is null)
        {
            return;
        }

        existing.Title = updateDto.Title;
        existing.Content = updateDto.Content;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int id)
    {
        await _dbContext.Comments.Where(c => c.CommentId == id).ExecuteDeleteAsync();
    }
}