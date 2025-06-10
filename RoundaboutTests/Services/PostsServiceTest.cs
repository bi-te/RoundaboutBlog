using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RoundaboutBlog.Data;
using RoundaboutBlog.Dto;
using RoundaboutBlog.Entities;
using RoundaboutBlog.Services;

namespace RoundaboutTests.Services;

[TestFixture]
[TestOf(typeof(PostsService))]
public class PostsServiceTest
{
  private SqliteConnection _connection;
  private AppDbContext _dbContext;
  private PostsService _postsService;
  private List<AppUser> _users;
  private List<Post> _posts;

  [SetUp]
  public void Setup()
  {
    _connection = new SqliteConnection("Filename=:memory:");
    _connection.Open();

    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite(_connection)
        .Options;

    _dbContext = new AppDbContext(options);
    _dbContext.Database.EnsureCreated();

    _users = new List<AppUser>
        {
            new AppUser { Id = "user1", UserName = "User 1" },
            new AppUser { Id = "user2", UserName = "User 2" }
        };
    _dbContext.Users.AddRange(_users);

    _posts = new List<Post>
        {
            new Post {
                PostId = 1,
                Title = "Post 1",
                Content = "Content 1",
                UserId = "user1",
                CreatedAt = DateTime.Now.AddDays(-1),
                User = _users[0]
            },
            new Post {
                PostId = 2,
                Title = "Post 2",
                Content = "Content 2",
                UserId = "user1",
                CreatedAt = DateTime.Now,
                User = _users[0]
            },
            new Post {
                PostId = 3,
                Title = "Post 3",
                Content = "Content 3",
                UserId = "user2",
                CreatedAt = DateTime.Now.AddDays(-2),
                User = _users[1]
            }
        };
    _dbContext.Posts.AddRange(_posts);
    _dbContext.SaveChanges();

    // Create service with test database
    _postsService = new PostsService(_dbContext);
  }

  [TearDown]
  public void TearDown()
  {
    _dbContext.Database.EnsureDeleted();
    _dbContext.Dispose();
    _connection.Dispose();
  }

  [Test]
  public async Task GetPostAsync_ExistingId_ReturnsPost()
  {
    var result = await _postsService.GetPostAsync(1);

    Assert.That(result, Is.Not.Null);
    Assert.That(result.PostId, Is.EqualTo(1));
    Assert.That(result.Title, Is.EqualTo("Post 1"));
    Assert.That(result.AuthorName, Is.Not.Null);
  }

  [Test]
  public async Task GetPostEntityAsync_NonExistingId_ReturnsNull()
  {
    var result = await _postsService.GetPostAsync(999);

    Assert.That(result, Is.Null);
  }

  [Test]
  public async Task GetPostsAsync_ReturnsAllPosts()
  {
    var result = await _postsService.GetPostsAsync();

    Assert.That(result, Is.Not.Null);
    Assert.That(result.Count, Is.EqualTo(3));
  }

  [Test]
  public async Task GetPostsSortedAsync_SortsByCreatedAtAscending_ReturnsOrderedPosts()
  {
    var result = await _postsService.GetPostsSortedAsync(p => p.CreatedAt);

    Assert.That(result, Is.Not.Null);
    Assert.That(result.Count, Is.EqualTo(3));

    // Posts should be in ascending order by CreatedAt
    var resultList = result.ToList();
    Assert.That(resultList[0].PostId, Is.EqualTo(3));
    Assert.That(resultList[1].PostId, Is.EqualTo(1));
    Assert.That(resultList[2].PostId, Is.EqualTo(2));
  }

  [Test]
  public async Task GetPostsSortedAsync_SortsByCreatedAtDescending_ReturnsOrderedPosts()
  {
    var result = await _postsService.GetPostsSortedAsync(p => p.CreatedAt, false);

    Assert.That(result, Is.Not.Null);
    Assert.That(result.Count, Is.EqualTo(3));

    // Posts should be in descending order by CreatedAt
    var resultList = result.ToList();
    Assert.That(resultList[0].PostId, Is.EqualTo(2));
    Assert.That(resultList[1].PostId, Is.EqualTo(1));
    Assert.That(resultList[2].PostId, Is.EqualTo(3));
  }

  [Test]
  public async Task GetPostsByAuthorAsync_ExistingAuthorId_ReturnsAuthorPosts()
  {
    var result = await _postsService.GetPostsByAuthorAsync("user1");

    Assert.That(result, Is.Not.Null);
    Assert.That(result.Count, Is.EqualTo(2));
    Assert.That(result.All(p => p.AuthorId == "user1"), Is.True);
  }

  [Test]
  public async Task GetPostsByAuthorAsync_NonExistingAuthorId_ReturnsEmptyCollection()
  {
    var result = await _postsService.GetPostsByAuthorAsync("nonexistent");

    Assert.That(result, Is.Not.Null);
    Assert.That(result.Count, Is.EqualTo(0));
  }

  [Test]
  public async Task AddPostAsync_ValidData_CreatesAndReturnsPost()
  {
    string userId = "user1";
    var createDto = new PostCreateDto { Title = "New Post", Content = "New Content" };

    var result = await _postsService.AddPostAsync(userId, createDto);

    Assert.That(result, Is.Not.Null);
    Assert.That(result.Title, Is.EqualTo("New Post"));
    Assert.That(result.Content, Is.EqualTo("New Content"));
    Assert.That(result.AuthorId, Is.EqualTo(userId));

    var posts = await _dbContext.Posts.ToListAsync();
    Assert.That(posts.Count, Is.EqualTo(4));
  }

  [Test]
  public async Task AddPostAsync_NonExistingUser_ReturnsNull()
  {
    string userId = "nonexistent";
    var createDto = new PostCreateDto { Title = "New Post", Content = "New Content" };

    var result = await _postsService.AddPostAsync(userId, createDto);

    Assert.That(result, Is.Null);

    var posts = await _dbContext.Posts.ToListAsync();
    Assert.That(posts.Count, Is.EqualTo(3));
  }

  [Test]
  public async Task UpdatePostAsync_ExistingPost_UpdatesPost()
  {
    int postId = 1;
    var updateDto = new PostCreateDto { Title = "Updated Title", Content = "Updated Content" };

    await _postsService.UpdatePostAsync(postId, updateDto);

    var updatedPost = await _dbContext.Posts.FindAsync(postId);
    Assert.That(updatedPost, Is.Not.Null);
    Assert.That(updatedPost.Title, Is.EqualTo("Updated Title"));
    Assert.That(updatedPost.Content, Is.EqualTo("Updated Content"));
  }

  [Test]
  public async Task UpdatePostAsync_NonExistingPost_DoesNothing()
  {
    int postId = 999;
    var updateDto = new PostCreateDto { Title = "Updated Title", Content = "Updated Content" };

    await _postsService.UpdatePostAsync(postId, updateDto);

    var posts = await _dbContext.Posts.ToListAsync();
    Assert.That(posts.Count, Is.EqualTo(3));
  }

  [Test]
  public async Task DeletePostAsync_ExistingPost_RemovesPost()
  {
    int postId = 1;
    await _postsService.DeletePostAsync(postId);

    var deletedPost = await _dbContext.Posts.Where(p => p.PostId == postId).SingleOrDefaultAsync();
    Assert.That(deletedPost, Is.Null);

    var remainingPosts = await _dbContext.Posts.ToListAsync();
    Assert.That(remainingPosts.Count, Is.EqualTo(2));

  }
}

