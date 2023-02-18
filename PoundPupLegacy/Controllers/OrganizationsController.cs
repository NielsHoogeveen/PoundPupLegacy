using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using System.Diagnostics;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Controllers;

[Route("organizations")]
public class OrganizationsController : Controller
{
    const int NUMBER_OF_ENTRIES = 50;

    const string TERM_NAME_PREFIX = "term-name-";

    private readonly IFetchOrganizationsService _fetchOrganizationsService;
    private readonly ILogger<OrganizationsController> _logger;
    private readonly ISiteDataService _siteDataService;

    public OrganizationsController(
        ILogger<OrganizationsController> logger,
        IFetchOrganizationsService fetchOrganizationsService,
        ISiteDataService siteDataService)
    {
        _fetchOrganizationsService = fetchOrganizationsService;
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

    public int? GetCountryId(string? country)
    {
        if (country is null || country == "0")
        {
            return null;
        }
        if (int.TryParse(country, out var countryId))
        {
            return countryId;
        }
        return null;
    }
    public int? GetOrganizationTypeId(string? organizationType)
    {
        if (organizationType is null || organizationType == "0")
        {
            return null;
        }
        if (int.TryParse(organizationType, out var organizationTypeId))
        {
            return organizationTypeId;
        }
        return null;
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
        int? countryId = GetCountryId(query["country"]);
        int? organizationTypeId = GetOrganizationTypeId(query["organization_type"]);
        var startIndex = (pageNumber - 1) * NUMBER_OF_ENTRIES;
        var organizations = await _fetchOrganizationsService.FetchOrganizations(NUMBER_OF_ENTRIES, startIndex, searchTerm, searchOption, organizationTypeId, countryId);
        organizations.Organizations.PageNumber = pageNumber;
        organizations.Organizations.NumberOfPages = (organizations.Organizations.NumberOfEntries / NUMBER_OF_ENTRIES) + 1;
        organizations.Organizations.QueryString = $"&search={searchTerm}";
        foreach (var country in organizations.Countries)
        {
            if (country.Id == countryId)
            {
                country.Selected = true;
                continue;
            }
            country.Selected = false;
        }
        foreach (var organizationType in organizations.OrganizationTypes)
        {
            if (organizationType.Id == organizationTypeId)
            {
                organizationType.Selected = true;
                continue;
            }
            organizationType.Selected = false;
        }
        organizations.Organizations.SelectedSearchOption = searchOption;
        organizations.Organizations.SearchTerm = searchTerm;
        _logger.LogInformation($"Fetched organizations in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return View("Organizations", organizations);
    }

}
