using Microsoft.AspNetCore.Authorization;

namespace RoundaboutBlog.Authorization.Requirements;

public class IsCommentOwnerRequirement : IAuthorizationRequirement
{
}