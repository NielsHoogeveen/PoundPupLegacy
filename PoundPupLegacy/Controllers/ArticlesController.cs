using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using System.Diagnostics;

namespace PoundPupLegacy.Controllers;

[Route("articles")]
public class ArticlesController : Controller
{
    const int NUMBER_OF_ENTRIES = 10;

    const string TERM_NAME_PREFIX = "term-name-";

    private readonly FetchArticlesService _fetchArticlesService;
    private readonly ILogger<ArticlesController> _logger;

    public ArticlesController(ILogger<ArticlesController> logger, FetchArticlesService fetchArticlesService)
    {
        _fetchArticlesService = fetchArticlesService;
        _logger = logger;
    }

    private IEnumerable<int> GetTermIds(IEnumerable<string> values)
    {
        foreach (var term in values) 
        {
            if (term.StartsWith(TERM_NAME_PREFIX))
            {
                var remainder = term.Substring(TERM_NAME_PREFIX.Length);
                if(int.TryParse(remainder, out int termId))
                {
                    yield return termId;
                }
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var pageNumber = 1;

        var query = this.HttpContext.Request.Query;
        var pageValue = query["page"];
        if (!string.IsNullOrEmpty(pageValue))
        {
            if (int.TryParse(pageValue, out int providedPageNumber))
            {
                pageNumber = providedPageNumber;
            }
        }
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var termIds = query == null? new List<int>(): GetTermIds(query.Keys).ToList();
        var startIndex = (pageNumber - 1) * NUMBER_OF_ENTRIES;
        var articles = termIds.Any() ? await _fetchArticlesService.GetArticles(termIds, startIndex, NUMBER_OF_ENTRIES) : await _fetchArticlesService.GetArticles((pageNumber - 1) * NUMBER_OF_ENTRIES, NUMBER_OF_ENTRIES);
        articles.PageNumber = pageNumber;
        articles.NumberOfPages = (articles.NumberOfEntries / NUMBER_OF_ENTRIES) + 1;
        articles.QueryString = string.Join("&", termIds.Select(x => $"{TERM_NAME_PREFIX}{x}"));
        _logger.LogInformation($"Fetched articles in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return View("Articles", articles);
    }

}
