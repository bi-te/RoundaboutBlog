using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RoundaboutBlog.Data;
using RoundaboutBlog.Dto;
using RoundaboutBlog.Entities;
using RoundaboutBlog.Mappings;

namespace RoundaboutBlog.Services;

public class PostsService(AppDbContext dbContext)
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<PostViewDto?> GetPostAsync(int postId)
    {
        return (await _dbContext.Posts.Where(p => p!.PostId == postId).SingleOrDefaultAsync())?.ToViewDto();
    }

    public async Task<ICollection<PostViewDto>> GetPostsAsync()
    {
        return await _dbContext.Posts.Select(p => p.ToViewDto()).ToListAsync();
    }
    
    public async Task<ICollection<PostViewDto>> GetPostsSortedAsync<TKey>(Expression<Func<Post?, TKey>> func, bool ascending = true)
    {
        if (ascending)
        {
            return await _dbContext.Posts.OrderBy(func).Select(p => p.ToViewDto()).ToListAsync();
        }
        return await _dbContext.Posts.OrderByDescending(func).Select(p => p.ToViewDto()).ToListAsync();
    }

    public async Task<PostViewDto> AddPostAsync(PostCreateDto createDto)
    {
        Post newPost = createDto.ToPost();
        
        var post = await _dbContext.AddAsync(newPost);
        await _dbContext.SaveChangesAsync();

        return post.Entity.ToViewDto();
    }

    public async Task UpdatePostAsync(int postId, PostCreateDto updateDto)
    {
        Post? existing = await _dbContext.Posts.FindAsync(postId);
        
        if (existing is null)
        {
            await AddPostAsync(updateDto);
            return;
        }

        existing.Title = updateDto.Title;
        existing.Content = updateDto.Content;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeletePostAsync(int postId)
    {
        await _dbContext.Posts.Where(p => p!.PostId == postId).ExecuteDeleteAsync();
    }
}