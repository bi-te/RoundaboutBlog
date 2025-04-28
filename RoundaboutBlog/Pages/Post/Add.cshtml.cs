#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Bcpg;
using RoundaboutBlog.Dto;
using RoundaboutBlog.Entities;
using RoundaboutBlog.Services;

namespace RoundaboutBlog.Pages.Post;

[Authorize]
public class AddModel : PageModel
{
    private readonly UserManager<AppUser> _userManager;
    private readonly PostsService _postsService;
    
    [BindProperty]
    public PostCreateDto Input { get; set; }

    public AddModel(UserManager<AppUser> userManager, PostsService postsService)
    {
        _userManager = userManager;
        _postsService = postsService;
    }
    
    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        string userId = _userManager.GetUserId(User);
        if (userId is null)
        {
            return Unauthorized();
        }
        
        await _postsService.AddPostAsync(userId, Input);
        return RedirectToPage("/Index");
    }
}