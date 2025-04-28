using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoundaboutBlog.Dto;
using RoundaboutBlog.Mappings;
using RoundaboutBlog.Services;

namespace RoundaboutBlog.Pages.Post;

public class IndexModel : PageModel
{
    private readonly IAuthorizationService _authorizationService;
    private readonly PostsService _postsService;
    
    public PostViewDto? Post { get; set; }
    public bool CanEdit { get; set; }

    public IndexModel(PostsService postsService, IAuthorizationService authorizationService)
    {
        _postsService = postsService;
        _authorizationService = authorizationService;
    }
    
    public async Task<IActionResult> OnGetAsync(int postId)
    {
        Post = await _postsService.GetPostAsync(postId);
        if (Post is null)
        {
            return NotFound();
        }
        
        AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, Post.ToPost(), "PostOwnerPolicy");
        CanEdit = result.Succeeded;
        
        return Page();
    }
}