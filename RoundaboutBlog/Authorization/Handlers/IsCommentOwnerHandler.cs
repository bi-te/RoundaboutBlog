using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RoundaboutBlog.Authorization.Requirements;
using RoundaboutBlog.Entities;

namespace RoundaboutBlog.Authorization.Handlers;

public class IsCommentOwnerHandler: AuthorizationHandler<IsCommentOwnerRequirement, Comment>
{
    private readonly UserManager<AppUser> _userManager;

    public IsCommentOwnerHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCommentOwnerRequirement requirement,
        Comment resource)
    {
        string? userId = _userManager.GetUserId(context.User);
        if (userId is null || resource.Post is null)
        {
            return Task.CompletedTask;
        }

        if (resource.UserId == userId || resource.Post.UserId == userId)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}