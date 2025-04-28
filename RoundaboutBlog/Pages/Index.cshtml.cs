using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoundaboutBlog.Dto;
using RoundaboutBlog.Entities;
using RoundaboutBlog.Services;

namespace RoundaboutBlog.Pages;

public class IndexModel : PageModel
{
    private readonly UserManager<AppUser> _userManager;
    private readonly PostsService _postsService;

    public string? Username;
    public ICollection<PostViewDto>? Posts;
    
    public IndexModel(PostsService postsService, UserManager<AppUser> userManager)
    {
        _postsService = postsService;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync(string? userId)
    {
        if (userId is null)
        {
            Posts = await _postsService.GetPostsSortedAsync(p => p.CreatedAt, false);
            return Page();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return NotFound($"No user found with ID {userId}");
        }
        
        Posts = await _postsService.GetPostsByAuthorAsync(userId);
        Username = user.UserName;
        return Page();
    }
}