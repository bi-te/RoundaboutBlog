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
        return (await _dbContext.Posts.Where(p => p.PostId == postId)
                                      .Include(p => p.User)
                                      .SingleOrDefaultAsync()
            )?.ToViewDto();
    }

    public async Task<ICollection<PostViewDto>> GetPostsAsync()
    {
        return await _dbContext.Posts.Include(p => p.User).Select(p => p.ToViewDto()).ToListAsync();
    }
    
    public async Task<ICollection<PostViewDto>> GetPostsSortedAsync<TKey>(Expression<Func<Post, TKey>> func, bool ascending = true)
    {
        if (ascending)
        {
            return await _dbContext.Posts.OrderBy(func).Include(p => p.User)
                                         .Select(p => p.ToViewDto()).ToListAsync();
        }
        return await _dbContext.Posts.OrderByDescending(func).Include(p => p.User)
                                     .Select(p => p.ToViewDto()).ToListAsync();
    }

    public async Task<PostViewDto?> AddPostAsync(string userId, PostCreateDto createDto)
    {
        AppUser? user = await _dbContext.Users.FindAsync(userId);
        if (user is null)
        {
            return null;
        }
        
        Post newPost = createDto.ToPost();
        newPost.UserId = userId;
        
        var post = await _dbContext.AddAsync(newPost);
        await _dbContext.SaveChangesAsync();

        post.Entity.User = user;
        return post.Entity.ToViewDto();
    }

    public async Task UpdatePostAsync(int postId, PostCreateDto updateDto)
    {
        Post? existing = await _dbContext.Posts.FindAsync(postId);
        
        if (existing is null)
        {
            //await AddPostAsync(TODO, updateDto);
            return;
        }

        existing.Title = updateDto.Title;
        existing.Content = updateDto.Content;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeletePostAsync(int postId)
    {
        await _dbContext.Posts.Where(p => p.PostId == postId).ExecuteDeleteAsync();
    }
}