﻿using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using PoundPupLegacy.Web.Services;
using System.Diagnostics;

namespace PoundPupLegacy.Controllers;

[Route("search")]
public class SearchController : Controller
{
    const int NUMBER_OF_ENTRIES = 25;

    private readonly FetchSearchService _fetchSearchService;
    private readonly ILogger<SearchController> _logger;
    private readonly SiteDataService _siteDataService;

    public SearchController(ILogger<SearchController> logger, FetchSearchService fetchSearchService, SiteDataService siteDataService)
    {
        _fetchSearchService = fetchSearchService;
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
        var searchValue = query["search"];
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
        var search = await _fetchSearchService.FetchSearch(NUMBER_OF_ENTRIES, startIndex, tenantId, userId, searchValue);
        search.PageNumber = pageNumber;
        search.NumberOfPages = (search.NumberOfEntries / NUMBER_OF_ENTRIES) + 1;
        _logger.LogInformation($"Fetched search in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return View("SearchResults", search);
    }

}