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

    public CountriesController(ILogger<CountriesController> logger, IFetchCountriesService fetchCountriesService, ISiteDataService siteDataService)
    {
        _fetchCountriesService = fetchCountriesService;
        _logger = logger;
        _siteDataService = siteDataService;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!_siteDataService.HasAccess()) {
            return NotFound();
        }
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var firstLevelRegions = await _fetchCountriesService.FetchCountries();
        _logger.LogInformation($"Fetched countries in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return View("FirstLevelRegions", firstLevelRegions);
    }
}
