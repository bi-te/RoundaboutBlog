namespace RoundaboutWebTests.Selenium;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System;
using System.IO;

[TestFixture]
[Category("Selenium")]
public class SeleniumTests
{
  private IWebDriver _driver;
  private WebDriverWait wait;

  [SetUp]
  public void Setup()
  {
    _driver = new ChromeDriver();
    wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
    Directory.CreateDirectory(" selenium_screenshots");
  }

  [TearDown]
  public void TearDown()
  {
    _driver.Quit();
    _driver.Dispose();
  }

  [Test, Order(1)]
  public void RegisterTest()
  {
    _driver.Navigate().GoToUrl("http://localhost:5154/Identity/Account/Login?ReturnUrl=%2F");
    wait.Until(d => d.FindElement(By.ClassName("alert")).Displayed);

    _driver.FindElement(By.LinkText("Register")).Click();

    _driver.FindElement(By.Id("Input_UserName")).SendKeys("user31");
    _driver.FindElement(By.Id("Input_Email")).SendKeys("user31@mail.com");
    _driver.FindElement(By.Id("Input_Password")).SendKeys("Test_user31");
    _driver.FindElement(By.Id("Input_ConfirmPassword")).SendKeys("Test_user31");

    _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

    try
    {
      wait.Until(d => d.FindElement(By.XPath("//*[contains(text(), 'Your registration has been')]")).Displayed);
      ( ( ITakesScreenshot )_driver ).GetScreenshot().SaveAsFile(" selenium_screenshots/registration_success.png");
    }
    catch ( Exception )
    {
      ( ( ITakesScreenshot )_driver ).GetScreenshot().SaveAsFile(" selenium_screenshots/registration_error.png");
      throw;
    }
  }

  [Test, Order(2)]
  public void ComplexTest()
  {
    // Login
    _driver.Navigate().GoToUrl("http://localhost:5154/Identity/Account/Login?ReturnUrl=%2F");
    _driver.FindElement(By.CssSelector("input[placeholder='name@example.com']")).SendKeys("user31@mail.com");
    _driver.FindElement(By.CssSelector("input[placeholder='password']")).SendKeys("Test_user31");
    _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

    try
    {
      wait.Until(d => d.FindElement(By.XPath("//h1[text()='Blog Posts']")).Displayed);
      ( ( ITakesScreenshot )_driver ).GetScreenshot().SaveAsFile(" selenium_screenshots/login_success.png");
    }
    catch ( Exception )
    {
      ( ( ITakesScreenshot )_driver ).GetScreenshot().SaveAsFile(" selenium_screenshots/login_error.png");
      throw;
    }

    // Add post
    _driver.FindElement(By.Id("dropdownMenuButton")).Click();
    _driver.FindElement(By.LinkText("Dashboard")).Click();
    _driver.FindElement(By.LinkText("Add Post")).Click();
    _driver.FindElement(By.Id("Title")).SendKeys("My First Test Post");
    _driver.FindElement(By.Id("Content")).SendKeys("Test Content");
    _driver.FindElement(By.XPath("//button[text()='Submit']")).Click();

    try
    {
      wait.Until(d => d.FindElement(By.XPath("//*[contains(text(), 'Post created successfully')]")).Displayed);
      ( ( ITakesScreenshot )_driver ).GetScreenshot().SaveAsFile(" selenium_screenshots/post_creation_success.png");
    }
    catch ( Exception )
    {
      ( ( ITakesScreenshot )_driver ).GetScreenshot().SaveAsFile(" selenium_screenshots/post_creation_error.png");
      throw;
    }

    // View post
    _driver.FindElement(By.LinkText("My First Test Post")).Click();

    // Add Comment
    _driver.FindElement(By.Id("Input_Title")).SendKeys("Test");
    _driver.FindElement(By.Id("Input_Content")).SendKeys("TestComment");
    _driver.FindElement(By.XPath("//button[text()='Add Comment']")).Click();

    try
    {
      wait.Until(d => d.FindElement(By.ClassName("alert")).Displayed);
      Assert.That(_driver.FindElement(By.TagName("main")).Text.Contains("Test"), Is.True);
      Assert.That(_driver.FindElement(By.TagName("main")).Text.Contains("TestComment"), Is.True);
      ( ( ITakesScreenshot )_driver ).GetScreenshot().SaveAsFile(" selenium_screenshots/comment_success.png");
    }
    catch ( Exception )
    {
      ( ( ITakesScreenshot )_driver ).GetScreenshot().SaveAsFile(" selenium_screenshots/comment_error.png");
      throw;
    }

    // Delete comment
    _driver.FindElement(By.ClassName("btn-danger")).Click();

    // Delete Post
    _driver.FindElement(By.LinkText("Edit")).Click();
    _driver.FindElement(By.ClassName("btn-danger")).Click();

    // // Edit profile
    _driver.FindElement(By.ClassName("dropdown-toggle")).Click();
    _driver.FindElement(By.LinkText("My Profile")).Click();

    var firstNameField = _driver.FindElement(By.CssSelector("input[placeholder='Please enter your first name.']"));
    firstNameField.Clear();
    firstNameField.SendKeys("Darius");

    _driver.FindElement(By.XPath("//button[text()='Save']")).Click();

    try
    {
      wait.Until(d => d.FindElement(By.ClassName("alert")).Displayed);
      string value = _driver.FindElement(By.CssSelector("input[placeholder='Please enter your first name.']")).GetAttribute("value");
      Assert.That(value, Is.EqualTo("Darius"));
      ( ( ITakesScreenshot )_driver ).GetScreenshot().SaveAsFile(" selenium_screenshots/profile_success.png");
    }
    catch ( Exception )
    {
      ( ( ITakesScreenshot )_driver ).GetScreenshot().SaveAsFile(" selenium_screenshots/profile_error.png");
      throw;
    }

    // Logout
    _driver.FindElement(By.Id("logout")).Click();
    wait.Until(d => d.FindElement(By.XPath("//h1[text()='Log in']")).Displayed);
  }

  [Test, Order(3)]
  public void DeleteUserTest()
  {
    // Login
    _driver.Navigate().GoToUrl("http://localhost:5154/Identity/Account/Login?ReturnUrl=%2F");
    _driver.FindElement(By.CssSelector("input[placeholder='name@example.com']")).SendKeys("user31@mail.com");
    _driver.FindElement(By.CssSelector("input[placeholder='password']")).SendKeys("Test_user31");
    _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
    wait.Until(d => d.FindElement(By.XPath("//h1[text()='Blog Posts']")).Displayed);

    // Delete user
    _driver.FindElement(By.Id("dropdownMenuButton")).Click();
    _driver.FindElement(By.LinkText("My Profile")).Click();
    _driver.FindElement(By.LinkText("Personal data")).Click();
    _driver.FindElement(By.XPath("//a[text()='Delete']")).Click();
    _driver.FindElement(By.CssSelector("input[placeholder='Please enter your password.']")).SendKeys("Test_user31");
    _driver.FindElement(By.XPath("//button[contains(text(), 'Delete data and close my')]")).Click();
    wait.Until(d => d.FindElement(By.XPath("//h1[text()='Log in']")).Displayed);
  }
}