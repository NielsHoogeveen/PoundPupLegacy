using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using System.Diagnostics;

namespace PoundPupLegacy.Controllers;

[Route("countries")]
public class CountriesController : Controller
{

    private readonly IFetchCountriesService _fetchCountriesService;
    private readonly ILogger<CountriesController> _logger;
    private readonly ISiteDataService _siteDataService;
    private readonly IUserService _userService;

    public CountriesController(
        ILogger<CountriesController> logger,
        IFetchCountriesService fetchCountriesService,
        ISiteDataService siteDataService,
        IUserService userService)
    {
        _fetchCountriesService = fetchCountriesService;
        _logger = logger;
        _siteDataService = siteDataService;
        _userService = userService;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = _userService.GetUserId(HttpContext.User);
        var tenantId = _siteDataService.GetTenantId(Request);
        if (!_siteDataService.HasAccess(userId, tenantId)) {
            return NotFound();
        }
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var firstLevelRegions = await _fetchCountriesService.FetchCountries(tenantId);
        _logger.LogInformation($"Fetched countries in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return View("FirstLevelRegions", firstLevelRegions);
    }
}
