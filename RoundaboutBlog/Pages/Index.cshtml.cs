using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoundaboutBlog.Dto;
using RoundaboutBlog.Services;

namespace RoundaboutBlog.Pages;

public class IndexModel : PageModel
{
    private readonly PostsService _postsService;

    public ICollection<PostViewDto>? Posts;
    
    public IndexModel(PostsService postsService)
    {
        _postsService = postsService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Posts = await _postsService.GetPostsSortedAsync(p => p!.CreatedAt, false);
        
        return Page();
    }
}