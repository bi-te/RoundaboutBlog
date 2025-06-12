using Microsoft.Playwright;

namespace RoundaboutWebTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Category("Playwright")]
public class UserTests : PageTest
{
  [Test, Order(1)]
  public async Task Test()
  {
    //Login
    await Page.GotoAsync("http://localhost:5154/Identity/Account/Login?ReturnUrl=%2F");
    await Page.GetByPlaceholder("name@example.com").ClickAsync();
    await Page.GetByPlaceholder("name@example.com").FillAsync("Voked1932@teleworm.us");
    await Page.GetByPlaceholder("password").ClickAsync();
    await Page.GetByPlaceholder("password").FillAsync("Test2131.");
    await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
    await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Blog Posts" })).ToBeVisibleAsync();
    
    // Add post
    await Page.Locator("li").Filter(new() { HasText = "bendy31 My Profile Dashboard" }).ClickAsync();
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
    await Page.GetByRole(AriaRole.Button, new() { Name = "bendy31" }).ClickAsync();
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
  
  // [Test]
  //   public async Task Test2()
  //   {
  //       await Page.GotoAsync("http://localhost:5154/Identity/Account/Login?ReturnUrl=%2F");
  //       
  //       
  //       await Page.GetByPlaceholder("name@example.com").ClickAsync();
  //       await Page.GetByPlaceholder("name@example.com").FillAsync("Voked1932@teleworm.us");
  //       await Page.GetByPlaceholder("password").ClickAsync();
  //       await Page.GetByPlaceholder("password").FillAsync("Test2131.");
  //       await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
  //       await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "RoundaboutBlog" })).ToBeVisibleAsync();
  //       await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Blog Posts" })).ToBeVisibleAsync();
  //       
  //       
  //       await Page.GetByRole(AriaRole.Button, new() { Name = "bendy31" }).ClickAsync();
  //       await Page.GetByRole(AriaRole.Link, new() { Name = "Add Post" }).ClickAsync();
  //       await Page.GetByRole(AriaRole.Main).Locator("form div").Filter(new() { HasText = "Title" }).ClickAsync();
  //       await Page.GetByLabel("Title").ClickAsync();
  //       await Page.GetByLabel("Title").FillAsync("TestPost");
  //       await Page.GetByLabel("Content").ClickAsync();
  //       await Page.GetByLabel("Content").FillAsync("TestContent");
  //       await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
  //       await Expect(Page.GetByText("Post created successfully")).ToBeVisibleAsync();
  //       await Expect(Page.GetByRole(AriaRole.Main)).ToContainTextAsync("TestPost");
  //       await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "TestPost" })).ToBeVisibleAsync();
  //       
  //       
  //       await Page.GetByRole(AriaRole.Link, new() { Name = "TestPost" }).ClickAsync();
  //       
  //       
  //       await Page.GetByLabel("Title").ClickAsync();
  //       await Page.GetByLabel("Title").FillAsync("TestComment");
  //       await Page.GetByRole(AriaRole.Main).Locator("form div").Filter(new() { HasText = "Comment" }).ClickAsync();
  //       await Page.GetByLabel("Comment").DblClickAsync();
  //       await Page.GetByLabel("Comment").FillAsync("TestCommentContent");
  //       await Page.GetByRole(AriaRole.Button, new() { Name = "Add Comment" }).ClickAsync();
  //       await Expect(Page.GetByText("Comment added successfully")).ToBeVisibleAsync();
  //       await Expect(Page.Locator("h6")).ToContainTextAsync("TestComment");
  //       await Page.GetByText("TestCommentContent").ClickAsync();
  //       
  //       
  //       // await Expect(Page.GetByText("TestComment Delete")).ToBeVisibleAsync();
  //       void Page_Dialog_EventHandler(object sender, IDialog dialog)
  //       {
  //           Console.WriteLine($"Dialog message: {dialog.Message}");
  //           dialog.DismissAsync();
  //           Page.Dialog -= Page_Dialog_EventHandler;
  //       }
  //       Page.Dialog += Page_Dialog_EventHandler;
  //       await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
  //       await Page.GetByRole(AriaRole.Link, new() { Name = "Edit" }).ClickAsync();
  //       void Page_Dialog1_EventHandler(object sender, IDialog dialog)
  //       {
  //           Console.WriteLine($"Dialog message: {dialog.Message}");
  //           dialog.DismissAsync();
  //           Page.Dialog -= Page_Dialog1_EventHandler;
  //       }
  //       Page.Dialog += Page_Dialog1_EventHandler;
  //       await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
  //       
  //       
  //       await Page.GetByRole(AriaRole.Button, new() { Name = "bendy31" }).ClickAsync();
  //       await Page.GetByRole(AriaRole.Link, new() { Name = "My Profile" }).ClickAsync();
  //       await Page.GetByPlaceholder("Please enter your first name.").FillAsync("Darius");
  //       await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
  //       await Expect(Page.GetByText("Your profile has been updated")).ToBeVisibleAsync();
  //       await Expect(Page.GetByPlaceholder("Please enter your first name.")).ToHaveValueAsync("Darius");
  //       
  //       
  //       await Page.GetByRole(AriaRole.Button, new() { Name = "Logout" }).ClickAsync();
  //       await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Log in", Exact = true })).ToBeVisibleAsync();
  //   }
  
}