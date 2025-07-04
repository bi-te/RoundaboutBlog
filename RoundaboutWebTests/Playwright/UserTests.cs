﻿using Microsoft.Playwright;

namespace RoundaboutWebTests;

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
    try
    {
      await Expect(Page.GetByText("Your registration has been")).ToBeVisibleAsync();
      await Page.ScreenshotAsync(new() { Path = "screenshots/registration_success.png" });
    }
    catch ( Exception e )
    {
      await Page.ScreenshotAsync(new() { Path = "screenshots/registration_error.png" });
      throw;
    }
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
    try
    {
      await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Blog Posts" })).ToBeVisibleAsync();
      await Page.ScreenshotAsync(new() { Path = "screenshots/login_success.png" });
    }
    catch ( Exception e )
    {
      await Page.ScreenshotAsync(new() { Path = "screenshots/login_error.png" });
      throw;
    }

    // Add post
    await Page.Locator("li").Filter(new() { HasText = "user31 My Profile Dashboard" }).ClickAsync();
    await Page.GetByRole(AriaRole.Link, new() { Name = "Add Post" }).ClickAsync();
    await Page.GetByLabel("Title").ClickAsync();
    await Page.GetByLabel("Title").FillAsync("My First Test Post");
    await Page.GetByLabel("Content").ClickAsync();
    await Page.GetByLabel("Content").FillAsync("Test Content");
    await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
    try
    {
      await Expect(Page.GetByText("Post created successfully")).ToBeVisibleAsync();
      await Page.ScreenshotAsync(new() { Path = "screenshots/post_creation_success.png" });
    }
    catch ( Exception e )
    {
      await Page.ScreenshotAsync(new() { Path = "screenshots/post_creation_error.png" });
      throw;
    }

    // View post
    await Page.GetByRole(AriaRole.Link, new() { Name = "My First Test Post" }).ClickAsync();

    // Add Comment
    await Page.GetByLabel("Title").ClickAsync();
    await Page.GetByLabel("Title").FillAsync("Test");
    await Page.GetByLabel("Comment").ClickAsync();
    await Page.GetByLabel("Comment").FillAsync("TestComment");
    await Page.GetByRole(AriaRole.Button, new() { Name = "Add Comment" }).ClickAsync();
    try
    {
      await Expect(Page.GetByText("Comment added successfully")).ToBeVisibleAsync();
      await Expect(Page.GetByRole(AriaRole.Main)).ToContainTextAsync("Test");
      await Expect(Page.GetByRole(AriaRole.Main)).ToContainTextAsync("TestComment");
      await Page.ScreenshotAsync(new() { Path = "screenshots/comment_success.png" });
    }
    catch ( Exception e )
    {
      await Page.ScreenshotAsync(new() { Path = "screenshots/comment_error.png" });
      throw;
    }

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
    try
    {
      await Expect(Page.GetByText("Your profile has been updated")).ToBeVisibleAsync();
      await Expect(Page.GetByPlaceholder("Please enter your first name.")).ToHaveValueAsync("Darius");
      await Page.ScreenshotAsync(new() { Path = "screenshots/profile_success.png" });
    }
    catch ( Exception e )
    {
      await Page.ScreenshotAsync(new() { Path = "screenshots/profile_error.png" });
      throw;
    }

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