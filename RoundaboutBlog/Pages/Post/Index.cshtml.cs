using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoundaboutBlog.Dto;
using RoundaboutBlog.Entities;
using RoundaboutBlog.Mappings;
using RoundaboutBlog.Services;

namespace RoundaboutBlog.Pages.Post;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IPostsService _postsService;
    private readonly ICommentsService _commentsService;
    
    public PostViewDto? Post { get; set; }
    
    public ICollection<CommentViewDto>? Comments { get; set; }
    
    public bool CanEdit { get; set; }
    
    [TempData]
    public string? StatusMessage { get; set; }
    
    [BindProperty]
    public CommentCreateDto Input { get; set; }

    public IndexModel(IPostsService postsService, IAuthorizationService authorizationService, ICommentsService commentsService)
    {
        _postsService = postsService;
        _authorizationService = authorizationService;
        _commentsService = commentsService;
    }
    
    public async Task<IActionResult> OnGetAsync(int postId)
    {
        return await UpdatePage(postId);
    }

    public async Task<IActionResult> OnPostAddCommentAsync(int postId)
    {
        if (!ModelState.IsValid)
        {
            return await UpdatePage(postId);
        }
        
        Post = await _postsService.GetPostAsync(postId);
        if (Post is null)
        {
            return NotFound();
        }

        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            return RedirectToPage("/Account/Login");
        }

        var comment = await _commentsService.AddComment(userId, postId, Input);
        StatusMessage = comment is not null ? "Comment added successfully" : "Error. Comment is not added";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(int postId, int commentId)
    {
        Comment? comment = await _commentsService.GetCommentEntityAsync(commentId);
        if (comment is null)
        {
            return NotFound();
        }

        AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, comment, "CommentOwnerPolicy");
        if (!result.Succeeded)
        {
            return Forbid();
        }

        await _commentsService.DeleteCommentAsync(commentId);
        return RedirectToPage();
    }
    
    private async Task<IActionResult> UpdatePage(int postId)
    {
        Post = await _postsService.GetPostAsync(postId);
        if (Post is null)
        {
            return NotFound();
        }

        Comments = await _commentsService.GetPostCommentsAsync(postId);
        
        AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, Post.ToPost(), "PostOwnerPolicy");
        CanEdit = result.Succeeded;
        
        return Page();
    }
}