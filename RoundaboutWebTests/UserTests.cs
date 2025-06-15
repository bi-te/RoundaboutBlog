using Microsoft.Playwright;

namespace RoundaboutWebTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Category("Playwright")]
public class UserTests : PageTest
{
  [Test, Order(1)]
  public async Task RegisterTest()
  {
    await Page.GotoAsync("http://localhost:5154/Identity/Account/Login?ReturnUrl=%2F");
    await Expect(Page.GetByText("Error. You must be logged in")).ToBeVisibleAsync();
    await Page.GetByRole(AriaRole.Link, new() { Name = "Register", Exact = true }).ClickAsync();
    await Page.GetByPlaceholder("username").ClickAsync();
    await Page.GetByPlaceholder("username").FillAsync("user31");
    await Page.GetByPlaceholder("name@example.com").ClickAsync();
    await Page.GetByPlaceholder("name@example.com").FillAsync("user31@mail.com");
    await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
    await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Test_user31");
    await Page.GetByLabel("Confirm Password").ClickAsync();
    await Page.GetByLabel("Confirm Password").FillAsync("Test_user31");
    await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
    await Expect(Page.GetByText("Your registration has been")).ToBeVisibleAsync();
  }
  
  [Test, Order(2)]
  public async Task ComplexTest()
  {
    //Login
    await Page.GotoAsync("http://localhost:5154/Identity/Account/Login?ReturnUrl=%2F");
    await Page.GetByPlaceholder("name@example.com").ClickAsync();
    await Page.GetByPlaceholder("name@example.com").FillAsync("user31@mail.com");
    await Page.GetByPlaceholder("password").ClickAsync();
    await Page.GetByPlaceholder("password").FillAsync("Test_user31");
    await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
    await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Blog Posts" })).ToBeVisibleAsync();

    // Add post
    await Page.Locator("li").Filter(new() { HasText = "user31 My Profile Dashboard" }).ClickAsync();
    await Page.GetByRole(AriaRole.Link, new() { Name = "Add Post" }).ClickAsync();
    await Page.GetByLabel("Title").ClickAsync();
    await Page.GetByLabel("Title").FillAsync("My First Test Post");
    await Page.GetByLabel("Content").ClickAsync();
    await Page.GetByLabel("Content").FillAsync("Test Content");
    await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
    await Expect(Page.GetByText("Post created successfully")).ToBeVisibleAsync();

    // View post
    await Page.GetByRole(AriaRole.Link, new() { Name = "My First Test Post" }).ClickAsync();

    // Add Comment
    await Page.GetByLabel("Title").ClickAsync();
    await Page.GetByLabel("Title").FillAsync("Test");
    await Page.GetByLabel("Comment").ClickAsync();
    await Page.GetByLabel("Comment").FillAsync("TestComment");
    await Page.GetByRole(AriaRole.Button, new() { Name = "Add Comment" }).ClickAsync();
    await Expect(Page.GetByText("Comment added successfully")).ToBeVisibleAsync();
    await Expect(Page.GetByRole(AriaRole.Main)).ToContainTextAsync("Test");
    await Expect(Page.GetByRole(AriaRole.Main)).ToContainTextAsync("TestComment");

    // Delete comment
    void PageDialogEventHandler(object sender, IDialog dialog)
    {
      Console.WriteLine($"Dialog message: {dialog.Message}");
      dialog.DismissAsync();
      Page.Dialog -= PageDialogEventHandler!;
    }
    Page.Dialog += PageDialogEventHandler!;
    await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();

    //Delete Post
    await Page.GetByRole(AriaRole.Link, new() { Name = "Edit" }).ClickAsync();
    await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();

    // Edit profile
    await Page.GetByRole(AriaRole.Button, new() { Name = "user31" }).ClickAsync();
    await Page.GetByRole(AriaRole.Link, new() { Name = "My Profile" }).ClickAsync();
    await Page.GetByPlaceholder("Please enter your first name.").ClickAsync();
    await Page.GetByPlaceholder("Please enter your first name.").FillAsync("Darius");
    await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
    await Expect(Page.GetByText("Your profile has been updated")).ToBeVisibleAsync();
    await Expect(Page.GetByPlaceholder("Please enter your first name.")).ToHaveValueAsync("Darius");

    //Logout
    await Page.GetByRole(AriaRole.Button, new() { Name = "Logout" }).ClickAsync();
    await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Log in", Exact = true })).ToBeVisibleAsync();
  }


  [Test, Order(3)]
  public async Task DeleteUserTest()
  {
    //Login
    await Page.GotoAsync("http://localhost:5154/Identity/Account/Login?ReturnUrl=%2F");
    await Page.GetByPlaceholder("name@example.com").ClickAsync();
    await Page.GetByPlaceholder("name@example.com").FillAsync("user31@mail.com");
    await Page.GetByPlaceholder("password").ClickAsync();
    await Page.GetByPlaceholder("password").FillAsync("Test_user31");
    await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
    await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Blog Posts" })).ToBeVisibleAsync();
    
    //Delete user
    await Page.GetByRole(AriaRole.Button, new() { Name = "user31" }).ClickAsync();
    await Page.GetByRole(AriaRole.Link, new() { Name = "My Profile" }).ClickAsync();
    await Page.GetByRole(AriaRole.Link, new() { Name = "Personal data" }).ClickAsync();
    await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
    await Page.GetByPlaceholder("Please enter your password.").ClickAsync();
    await Page.GetByPlaceholder("Please enter your password.").FillAsync("Test_user31");
    await Page.GetByRole(AriaRole.Button, new() { Name = "Delete data and close my" }).ClickAsync();
    await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Log in", Exact = true })).ToBeVisibleAsync();
  }
}