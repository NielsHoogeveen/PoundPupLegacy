using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using System.Diagnostics;

namespace PoundPupLegacy.Controllers;

[Route("polls")]
public sealed class PollsController : Controller
{
    const int NUMBER_OF_ENTRIES = 25;

    const string TERM_NAME_PREFIX = "term-name-";

    private readonly IFetchPollsService _fetchPollsService;
    private readonly ILogger<PollsController> _logger;
    private readonly ISiteDataService _siteDataService;
    private readonly IUserService _userService;

    public PollsController(
        ILogger<PollsController> logger,
        IFetchPollsService fetchPollsService,
        ISiteDataService siteDataService,
        IUserService userService)
    {
        _fetchPollsService = fetchPollsService;
        _siteDataService = siteDataService;
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        //if (!_siteDataService.HasAccess())
        //{
        //    return NotFound();
        //}
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
        var startIndex = (pageNumber - 1) * NUMBER_OF_ENTRIES;
        var userId = _userService.GetUserId(HttpContext.User);
        var tenantId = _siteDataService.GetTenantId(Request);
        var articles = await _fetchPollsService.GetPolls(userId, tenantId, NUMBER_OF_ENTRIES, startIndex);
        articles.PageNumber = pageNumber;
        articles.NumberOfPages = (articles.NumberOfEntries / NUMBER_OF_ENTRIES) + 1;
        articles.QueryString = "";
        _logger.LogInformation($"Fetched polls in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return View("Polls", articles);
    }

}
