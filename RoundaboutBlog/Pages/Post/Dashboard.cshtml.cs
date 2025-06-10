using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoundaboutBlog.Dto;
using RoundaboutBlog.Entities;
using RoundaboutBlog.Services;

namespace RoundaboutBlog.Pages.Post;

[Authorize]
public class DashboardModel : PageModel
{
  private readonly UserManager<AppUser> _userManager;
  private readonly IPostsService _postsService;

  public string? Username;
  public ICollection<PostViewDto>? Posts;

  public DashboardModel(IPostsService postsService, UserManager<AppUser> userManager)
  {
    _userManager = userManager;
    _postsService = postsService;
  }

  public async Task<IActionResult> OnGetAsync()
  {
    AppUser? user = await _userManager.GetUserAsync(User);
    if ( user is null )
    {
      return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
    }

    Username = user.UserName;
    Posts = await _postsService.GetPostsByAuthorAsync(user.Id);

    return Page();
  }
}