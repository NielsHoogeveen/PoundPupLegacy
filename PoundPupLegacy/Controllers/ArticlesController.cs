using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using System.Diagnostics;

namespace PoundPupLegacy.Controllers;

[Route("articles")]
public class ArticlesController : Controller
{
    const int NUMBER_OF_ENTRIES = 10;

    const string TERM_NAME_PREFIX = "term-name-";

    private readonly IFetchArticlesService _fetchArticlesService;
    private readonly ILogger<ArticlesController> _logger;
    private readonly ISiteDataService _siteDataService;
    private readonly IUserService _userService;

    public ArticlesController(
        ILogger<ArticlesController> logger,
        IFetchArticlesService fetchArticlesService,
        ISiteDataService siteDataService,
        IUserService userService)
    {
        _fetchArticlesService = fetchArticlesService;
        _siteDataService = siteDataService;
        _logger = logger;
        _userService = userService;
    }

    private IEnumerable<int> GetTermIds(IEnumerable<string> values)
    {
        foreach (var term in values) {
            if (term.StartsWith(TERM_NAME_PREFIX)) {
                var remainder = term.Substring(TERM_NAME_PREFIX.Length);
                if (int.TryParse(remainder, out int termId)) {
                    yield return termId;
                }
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = _userService.GetUserId(HttpContext.User);
        var tenantId = _siteDataService.GetTenantId(Request);
        if (!_siteDataService.HasAccess(userId, tenantId)) {
            return NotFound();
        }
        var pageNumber = 1;

        var query = this.HttpContext.Request.Query;
        var pageValue = query["page"];
        if (!string.IsNullOrEmpty(pageValue)) {
            if (int.TryParse(pageValue, out int providedPageNumber)) {
                pageNumber = providedPageNumber;
            }
        }
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var termIds = query == null ? new List<int>() : GetTermIds(query.Keys).ToList();
        var startIndex = (pageNumber - 1) * NUMBER_OF_ENTRIES;
        var articles = termIds.Any() ? await _fetchArticlesService.GetArticles(tenantId, termIds, startIndex, NUMBER_OF_ENTRIES) : await _fetchArticlesService.GetArticles(tenantId, (pageNumber - 1) * NUMBER_OF_ENTRIES, NUMBER_OF_ENTRIES);
        articles.PageNumber = pageNumber;
        articles.NumberOfPages = (articles.NumberOfEntries / NUMBER_OF_ENTRIES) + 1;
        articles.QueryString = string.Join("&", termIds.Select(x => $"{TERM_NAME_PREFIX}{x}"));
        _logger.LogInformation($"Fetched articles in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return View("Articles", articles);
    }

}
