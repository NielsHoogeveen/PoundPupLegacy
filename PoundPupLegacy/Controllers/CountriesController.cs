using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using PoundPupLegacy.Web.Services;
using System.Diagnostics;

namespace PoundPupLegacy.Controllers;

[Route("countries")]
public class CountriesController : Controller
{

    private readonly FetchCountriesService _fetchCountriesService;
    private readonly ILogger<CountriesController> _logger;
    private readonly SiteDataService _siteDataService;

    public CountriesController(ILogger<CountriesController> logger, FetchCountriesService fetchCountriesService, SiteDataService siteDataService)
    {
        _fetchCountriesService = fetchCountriesService;
        _logger = logger;
        _siteDataService = siteDataService;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!_siteDataService.HasAccess(HttpContext))
        {
            return NotFound();
        }
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var firstLevelRegions = await _fetchCountriesService.FetchCountries(HttpContext);
        _logger.LogInformation($"Fetched countries in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return View("FirstLevelRegions", firstLevelRegions);
    }
}
