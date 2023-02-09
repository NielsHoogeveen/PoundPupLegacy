using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using System.Diagnostics;

namespace PoundPupLegacy.Controllers;

[Route("blog")]
public class BlogController : Controller
{
    const int NUMBER_OF_ENTRIES = 10;
    private IFetchBlogService _fetchBlogService;
    private readonly ILogger<BlogController> _logger;

    public BlogController(ILogger<BlogController> logger, IFetchBlogService fetchBlogService)
    {
        _logger = logger;
        _fetchBlogService = fetchBlogService;
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlog (int id)
    {
        var pageNumber = 1;
        var pageValue = HttpContext.Request.Query["page"];
        if (!string.IsNullOrEmpty(pageValue))
        {
            if (int.TryParse(pageValue, out int providedPageNumber))
            {
                pageNumber = providedPageNumber;
            }
        }
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var blog = await _fetchBlogService.FetchBlog(HttpContext, id, (pageNumber - 1) * NUMBER_OF_ENTRIES, NUMBER_OF_ENTRIES);
        blog.Id = id;
        blog.PageNumber = pageNumber;
        blog.NumberOfPages = (blog.NumberOfEntries / NUMBER_OF_ENTRIES) + 1;
        _logger.LogInformation($"Fetched node {id} in {stopwatch.Elapsed.TotalMilliseconds} ms");

        return View("Blog", blog);
    }
}
