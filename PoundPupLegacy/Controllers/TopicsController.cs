﻿using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using System.Diagnostics;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Controllers;

[Route("topics")]
public class TopicsController : Controller
{
    const int NUMBER_OF_ENTRIES = 50;

    private readonly ITopicService _topicService;
    private readonly ILogger<TopicsController> _logger;
    private readonly ISiteDataService _siteDataService;

    public TopicsController(
        ILogger<TopicsController> logger,
        ITopicService topicService,
        ISiteDataService siteDataService)
    {
        _topicService = topicService;
        _siteDataService = siteDataService;
        _logger = logger;
    }

    public SearchOption GetSearchOption(string? term)
    {
        return term switch
        {
            null => SearchOption.Contains,
            "contains" => SearchOption.Contains,
            "starts_with" => SearchOption.StartsWith,
            "ends_with" => SearchOption.EndsWith,
            "is_equal_to" => SearchOption.IsEqualTo,
            _ => SearchOption.Contains,
        };
    }

    public string GetSearchTerm(string? term)
    {
        if (term is null)
        {
            return "";
        }
        return term;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!_siteDataService.HasAccess())
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
        var searchTerm = GetSearchTerm(query["search"]);
        var searchOption = GetSearchOption(query["search_option"]);
        var startIndex = (pageNumber - 1) * NUMBER_OF_ENTRIES;
        var topics = await _topicService.FetchTopics(NUMBER_OF_ENTRIES, startIndex, searchTerm, searchOption);
        topics.PageNumber = pageNumber;
        topics.NumberOfPages = (topics.NumberOfEntries / NUMBER_OF_ENTRIES) + 1;
        topics.QueryString = $"&search={searchTerm}";
        topics.SelectedSearchOption = searchOption;
        topics.SearchTerm = searchTerm;
        _logger.LogInformation($"Fetched organizations in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return View("Topics", topics);
    }

}
