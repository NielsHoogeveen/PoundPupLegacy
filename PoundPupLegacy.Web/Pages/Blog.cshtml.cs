using Microsoft.AspNetCore.Mvc.RazorPages;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.Web.Services;
using System.Diagnostics;

namespace PoundPupLegacy.Web.Pages;

public class BlogModel : PageModel
{
    const int NUMBER_OF_ENTRIES = 10;
    public int PageNumber { get; set; } = 1;
    public int NumberOfPages { get; set; }
    public Blog? Blog { get; set; }
    public int Id { get; set; }
    private FetchBlogService _fetchBlogService;
    private readonly ILogger<BlogModel> _logger;
    public BlogModel(ILogger<BlogModel> logger, FetchBlogService fetchBlogService)
    {
        _logger = logger;
        _fetchBlogService = fetchBlogService;
    }



    public async Task OnGet(int id)
    {
        Id = id;
        var pageValue = HttpContext.Request.Query["page"];
        if (!string.IsNullOrEmpty(pageValue))
        {
            if(int.TryParse(pageValue, out int pageNumber))
            {
                PageNumber = pageNumber;
            }
        }
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        Blog = await _fetchBlogService.FetchBlog(HttpContext, id, (PageNumber - 1) * NUMBER_OF_ENTRIES, NUMBER_OF_ENTRIES);
        NumberOfPages = (Blog.Value.NumberOfEntries / 10) + 1;
        _logger.LogInformation($"Fetched node {id} in {stopwatch.Elapsed.TotalMilliseconds} ms");

    }
}
