using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RoundaboutBlog.Authorization.Requirements;
using RoundaboutBlog.Entities;

namespace RoundaboutBlog.Authorization.Handlers;

public class IsPostOwnerHandler : AuthorizationHandler<IsPostOwnerRequirement, Post>
{
  private readonly UserManager<AppUser> _userManager;

  public IsPostOwnerHandler(UserManager<AppUser> userManager)
  {
    _userManager = userManager;
  }

  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsPostOwnerRequirement requirement, Post resource)
  {
    string? userId = _userManager.GetUserId(context.User);
    if ( userId == null )
    {
      return Task.CompletedTask;
    }
    if ( userId == resource.UserId )
    {
      context.Succeed(requirement);
    }

    return Task.CompletedTask;
  }
}