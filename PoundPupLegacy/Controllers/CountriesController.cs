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

    public CountriesController(ILogger<CountriesController> logger, FetchCountriesService fetchCountriesService)
    {
        _fetchCountriesService = fetchCountriesService;
        _logger = logger;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var firstLevelRegions = await _fetchCountriesService.FetchCountries(HttpContext);
        _logger.LogInformation($"Fetched countries in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return View("FirstLevelRegions", firstLevelRegions);
    }
}
