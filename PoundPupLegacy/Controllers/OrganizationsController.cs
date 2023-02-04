using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using PoundPupLegacy.Web.Services;
using System.Diagnostics;
using PoundPupLegacy.ViewModel;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Controllers;

[Route("organizations")]
public class OrganizationsController : Controller
{
    const int NUMBER_OF_ENTRIES = 50;

    const string TERM_NAME_PREFIX = "term-name-";

    private readonly FetchOrganizationsService _fetchOrganizationsService;
    private readonly ILogger<OrganizationsController> _logger;
    private readonly SiteDataService _siteDataService;

    public OrganizationsController(ILogger<OrganizationsController> logger, FetchOrganizationsService fetchOrganizationsService, SiteDataService siteDataService)
    {
        _fetchOrganizationsService = fetchOrganizationsService;
        _siteDataService = siteDataService;
        _logger = logger;
    }

    private IEnumerable<int> GetTermIds(IEnumerable<string> values)
    {
        foreach (var term in values) 
        {
            if (term.StartsWith(TERM_NAME_PREFIX))
            {
                var remainder = term.Substring(TERM_NAME_PREFIX.Length);
                if(int.TryParse(remainder, out int termId))
                {
                    yield return termId;
                }
            }
        }
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
        if(term is null)
        {
            return "";
        }
        return term;
    }

    public int? GetCountryId(string? country)
    {
        if(country is null)
        {
            return null;
        }
        if(int.TryParse(country, out var countryId))
        {
            return countryId;
        }
        return null;
    }
    public int? GetOrganizationTypeId(string? organizationType)
    {
        if (organizationType is null)
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
        int? organizationTypeId = GetCountryId(query["organization_type"]);
        var startIndex = (pageNumber - 1) * NUMBER_OF_ENTRIES;
        var userId = _siteDataService.GetUserId(HttpContext.User);
        var tenantId = _siteDataService.GetTenantId(HttpContext.Request.Host.Value);
        var organizations = await _fetchOrganizationsService.FetchOrganizations(NUMBER_OF_ENTRIES, startIndex, searchTerm, searchOption, tenantId!.Value, userId);
        organizations.PageNumber = pageNumber;
        organizations.NumberOfPages = (organizations.NumberOfEntries / NUMBER_OF_ENTRIES) + 1;
        organizations.QueryString = $"&search={searchTerm}";
        foreach(var country in organizations.Countries)
        {
            if(country.Id == countryId)
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
        organizations.SelectedSearchOption = searchOption;
        organizations.SearchTerm = searchTerm;
        _logger.LogInformation($"Fetched organizations in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return View("Organizations", organizations);
    }

}
