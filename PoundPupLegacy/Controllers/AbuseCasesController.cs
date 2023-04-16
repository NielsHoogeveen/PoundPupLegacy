using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using PoundPupLegacy.ViewModel.Models;
using System.Diagnostics;

namespace PoundPupLegacy.Controllers;

[Route("abuse_cases")]
public sealed class AbuseCasesController : Controller
{
    const int NUMBER_OF_ENTRIES = 25;

    private readonly IFetchCasesService _fetchCasesService;
    private readonly ILogger<AbuseCasesController> _logger;
    private readonly ISiteDataService _siteDataService;
    private readonly IUserService _userService;

    public AbuseCasesController(
        ILogger<AbuseCasesController> logger,
        IFetchCasesService fetchCasesService,
        ISiteDataService siteDataService,
        IUserService userService)
    {
        _fetchCasesService = fetchCasesService;
        _siteDataService = siteDataService;
        _logger = logger;
        _userService = userService;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = _userService.GetUserId(HttpContext.User);
        var tenantId = _siteDataService.GetTenantId(Request);
        if (!_siteDataService.HasAccess(userId, tenantId, Request)) {
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
        var startIndex = (pageNumber - 1) * NUMBER_OF_ENTRIES;
        var cases = await _fetchCasesService.FetchCases(NUMBER_OF_ENTRIES, startIndex, tenantId, userId, CaseType.Abuse);
        cases.PageNumber = pageNumber;
        cases.NumberOfPages = (cases.NumberOfEntries / NUMBER_OF_ENTRIES) + 1;
        _logger.LogInformation($"Fetched cases in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return View("AbuseCases", cases);
    }

}
