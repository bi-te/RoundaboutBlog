using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoundaboutBlog.Dto;
using RoundaboutBlog.Services;

namespace RoundaboutBlog.Pages.Post;

public class AddModel : PageModel
{
    private PostsService _postsService;
    
    [BindProperty]
    public PostCreateDto Input { get; set; }

    public AddModel(PostsService postsService)
    {
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

        await _postsService.AddPostAsync(Input);
        return RedirectToPage("/Index");
    }
}