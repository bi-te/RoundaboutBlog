#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoundaboutBlog.Dto;
using RoundaboutBlog.Mappings;
using RoundaboutBlog.Services;

namespace RoundaboutBlog.Pages.Post;

[Authorize]
public class EditModel : PageModel
{
  private readonly IAuthorizationService _authorizationService;
  private readonly IPostsService _postsService;

  [BindProperty]
  public PostCreateDto Input { get; set; }

  public PostViewDto Post { get; set; }

  public int Id { get; set; }

  public EditModel(IPostsService postsService, IAuthorizationService authorizationService)
  {
    _postsService = postsService;
    _authorizationService = authorizationService;
  }

  public async Task<IActionResult> OnGetAsync(int postId)
  {
    Id = postId;
    Post = await _postsService.GetPostAsync(postId);
    if ( Post is null )
    {
      return NotFound();
    }

    AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, Post.ToPost(), "PostOwnerPolicy");
    if ( !result.Succeeded )
    {
      return Forbid();
    }

    Input = new PostCreateDto { Title = Post.Title, Content = Post.Content };
    return Page();
  }

  public async Task<IActionResult> OnPostAsync(int postId)
  {
    Id = postId;
    Post = await _postsService.GetPostAsync(postId);
    if ( Post is null )
    {
      return NotFound();
    }

    AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, Post.ToPost(), "PostOwnerPolicy");
    if ( !result.Succeeded )
    {
      return Forbid();
    }

    if ( !ModelState.IsValid )
    {
      return Page();
    }

    await _postsService.UpdatePostAsync(postId, Input);

    Post = await _postsService.GetPostAsync(postId);
    return Page();
  }

  public async Task<IActionResult> OnPostDeleteAsync(int postId)
  {
    Id = postId;
    Post = await _postsService.GetPostAsync(postId);
    if ( Post is null )
    {
      return NotFound();
    }

    AuthorizationResult result = await _authorizationService.AuthorizeAsync(User, Post.ToPost(), "PostOwnerPolicy");
    if ( !result.Succeeded )
    {
      return Forbid();
    }

    await _postsService.DeletePostAsync(postId);
    return RedirectToPage("/Index");
  }
}