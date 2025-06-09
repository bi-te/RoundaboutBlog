using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using RoundaboutBlog.Dto;
using RoundaboutBlog.Entities;
using RoundaboutBlog.Pages.Post;
using RoundaboutBlog.Services;

namespace RoundaboutTests.Pages.Post;

[TestFixture]
[TestOf(typeof(IndexModel))]
public class IndexModelTests
{
    private Mock<IPostsService> _mockPostsService;
    private Mock<ICommentsService> _mockCommentsService;
    private Mock<IAuthorizationService> _mockAuthService;
    private IndexModel _pageModel;
    private TestTempDataProvider _tempDataProvider;

    [SetUp]
    public void Setup()
    {
        _mockPostsService = new Mock<IPostsService>();
        _mockCommentsService = new Mock<ICommentsService>();
        _mockAuthService = new Mock<IAuthorizationService>();
        
        _pageModel = new IndexModel(
            _mockPostsService.Object,
            _mockAuthService.Object,
            _mockCommentsService.Object
        );
        
        _tempDataProvider = new TestTempDataProvider();
        var tempData = new TempDataDictionary(new DefaultHttpContext(), _tempDataProvider);
        _pageModel.TempData = tempData;
        
        _pageModel.ModelState.Clear();
    }

    [Test]
    public async Task OnGetAsync_ExistingPost_ReturnsPageResult()
    {
        const int postId = 1;
        var post = new PostViewDto
        {
            PostId = postId, 
            Title = "Test Post", 
            Content = "Test Content", 
            AuthorId = "0"
        };
        var comments = new List<CommentViewDto> { new()
        {
            CommentId = 1, 
            Title = "Test Comment", 
            Content = "Test Content", 
            AuthorId = "0"
        } };
        
        _mockPostsService.Setup(s => s.GetPostAsync(postId)).ReturnsAsync(post);
        _mockCommentsService.Setup(s => s.GetPostCommentsAsync(postId)).ReturnsAsync(comments);
        _mockAuthService.Setup(s => s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), "PostOwnerPolicy"))
            .ReturnsAsync(AuthorizationResult.Success());
        
        var result = await _pageModel.OnGetAsync(postId);
        
        Assert.That(result, Is.InstanceOf<PageResult>());
        Assert.That(_pageModel.Post, Is.Not.Null);
        Assert.That(_pageModel.Post.PostId, Is.EqualTo(postId));
        Assert.That(_pageModel.Comments, Is.Not.Null);
        Assert.That(_pageModel.CanEdit, Is.True);
    }

    [Test]
    public async Task OnGetAsync_NonExistingPost_ReturnsNotFound()
    {
        const int postId = 999;
        _mockPostsService.Setup(s => s.GetPostAsync(postId)).ReturnsAsync((PostViewDto)null!);
        
        var result = await _pageModel.OnGetAsync(postId);
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task OnPostAddCommentAsync_ValidModelAndExistingPost_RedirectsToPage()
    {
        const int postId = 1;
        var post = new PostViewDto
        {
            PostId = postId, 
            Title = "Test Post", 
            Content = "Test Content", 
            AuthorId = "0"
        };
        var comment = new CommentViewDto
        {
            CommentId = 1, 
            Title = "Test Comment", 
            Content = "Test Content", 
            AuthorId = "0"
        };
        var input = new CommentCreateDto
        {
            Title = "Test Post", 
            Content = "Test Content"
        };
        
        _mockPostsService.Setup(s => s.GetPostAsync(postId)).ReturnsAsync(post);
        _mockCommentsService.Setup(s => s.AddComment(It.IsAny<string>(), postId, It.IsAny<CommentCreateDto>()))
            .ReturnsAsync(comment);
        
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "user1"),
        }, "mock"));
        
        _pageModel.PageContext = new PageContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
        _pageModel.Input = input;
        
        var result = await _pageModel.OnPostAddCommentAsync(postId);
        
        Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        Assert.That(_pageModel.StatusMessage, Is.EqualTo("Comment added successfully"));
    }

    [Test]
    public async Task OnPostDeleteCommentAsync_ExistingCommentAndAuthorized_RedirectsToPage()
    {
        const int postId = 1;
        const int commentId = 1;
        var comment = new Comment
        {
            Title = "Test Comment", 
            Content = "Test Content", 
            CommentId = commentId, PostId = postId
        };
        
        _mockCommentsService.Setup(s => s.GetCommentEntityAsync(commentId)).ReturnsAsync(comment);
        _mockAuthService.Setup(s => s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), comment, "CommentOwnerPolicy"))
            .ReturnsAsync(AuthorizationResult.Success());
        
        // Act
        var result = await _pageModel.OnPostDeleteCommentAsync(postId, commentId);
        
        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        _mockCommentsService.Verify(s => s.DeleteCommentAsync(commentId), Times.Once);
    }

    [Test]
    public async Task OnPostDeleteCommentAsync_NonExistingComment_ReturnsNotFound()
    {
        const int postId = 1;
        const int commentId = 999;
        
        _mockCommentsService.Setup(s => s.GetCommentEntityAsync(commentId)).ReturnsAsync((Comment)null!);
        
        var result = await _pageModel.OnPostDeleteCommentAsync(postId, commentId);
        
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
        _mockCommentsService.Verify(s => s.DeleteCommentAsync(It.IsAny<int>()), Times.Never);
    }

    [Test]
    public async Task OnPostDeleteCommentAsync_Unauthorized_ReturnsForbid()
    {
        const int postId = 1;
        const int commentId = 1;
        var comment = new Comment
        {
            Title = "Test Comment", 
            Content = "Test Content", 
            CommentId = commentId, PostId = postId
        };
        
        _mockCommentsService.Setup(s => s.GetCommentEntityAsync(commentId)).ReturnsAsync(comment);
        _mockAuthService.Setup(s => s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), comment, "CommentOwnerPolicy"))
            .ReturnsAsync(AuthorizationResult.Failed());
        
        var result = await _pageModel.OnPostDeleteCommentAsync(postId, commentId);
        
        Assert.That(result, Is.InstanceOf<ForbidResult>());
        _mockCommentsService.Verify(s => s.DeleteCommentAsync(It.IsAny<int>()), Times.Never);
    }
}

public class TestTempDataProvider : ITempDataProvider
{
    private Dictionary<string, object> _data = new Dictionary<string, object>();
    
    public IDictionary<string, object> LoadTempData(HttpContext context)
    {
        return _data;
    }
    
    public void SaveTempData(HttpContext context, IDictionary<string, object> values)
    {
        _data = new Dictionary<string, object>(values);
    }
}
