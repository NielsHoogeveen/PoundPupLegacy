using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using System.Diagnostics;

namespace PoundPupLegacy.Controllers;

[Route("coerced_adoption_cases")]
public class CoercedAdoptionCasesController : Controller
{
    const int NUMBER_OF_ENTRIES = 25;

    private readonly IFetchCasesService _fetchCasesService;
    private readonly ILogger<CoercedAdoptionCasesController> _logger;
    private readonly ISiteDataService _siteDataService;

    public CoercedAdoptionCasesController(
        ILogger<CoercedAdoptionCasesController> logger,
        IFetchCasesService fetchCasesService,
        ISiteDataService siteDataService)
    {
        _fetchCasesService = fetchCasesService;
        _siteDataService = siteDataService;
        _logger = logger;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!_siteDataService.HasAccess(HttpContext))
        {
            return NotFound();
        }
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
        var startIndex = (pageNumber - 1) * NUMBER_OF_ENTRIES;
        var userId = _siteDataService.GetUserId(HttpContext);
        var tenantId = _siteDataService.GetTenantId(HttpContext);
        var cases = await _fetchCasesService.FetchCases(NUMBER_OF_ENTRIES, startIndex, tenantId, userId, ViewModel.CaseType.CoercedAdoption);
        cases.PageNumber = pageNumber;
        cases.NumberOfPages = (cases.NumberOfEntries / NUMBER_OF_ENTRIES) + 1;
        _logger.LogInformation($"Fetched cases in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return View("CoercedAdoptionCases", cases);
    }

}
