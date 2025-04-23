using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RoundaboutBlog.Pages.Post;

public class Test : PageModel
{
    public string? GeneratedUrl { get; set; }
    
    public IActionResult OnGet()
    {
        GeneratedUrl = Url.Page("Index");
        return RedirectToPage("/Index");
    }
}