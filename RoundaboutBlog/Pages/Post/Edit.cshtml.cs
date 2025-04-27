using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoundaboutBlog.Dto;
using RoundaboutBlog.Services;

namespace RoundaboutBlog.Pages.Post;

[Authorize]
public class EditModel : PageModel
{
    private PostsService _postsService;
    
    [BindProperty]
    public PostCreateDto Input { get; set; }
    
    public PostViewDto? Data { get; set; }
    
    public int Id { get; set; }

    public EditModel(PostsService postsService)
    {
        _postsService = postsService;
    }
    
    public async Task<IActionResult> OnGetAsync(int postId)
    {
        Id = postId;
        Data = await _postsService.GetPostAsync(postId);
        if (Data is null)
        {
            return NotFound();
        }

        Input = new PostCreateDto{ Title = Data.Title, Content = Data.Content };

        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(int postId)
    {
        Id = postId;
        Data = await _postsService.GetPostAsync(postId);
        if (Data is null)
        {
            return NotFound();
        }
        
        if (!ModelState.IsValid)
        {
            return Page();
        }
    
        await _postsService.UpdatePostAsync(postId, Input);
        
        Data = await _postsService.GetPostAsync(postId);
        return Page();
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(int postId)
    {
        await _postsService.DeletePostAsync(postId);
        return RedirectToPage("/Index");
    }
}