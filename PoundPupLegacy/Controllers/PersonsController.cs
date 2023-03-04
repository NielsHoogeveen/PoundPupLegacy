using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using System.Diagnostics;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Controllers;

[Route("persons")]
public class PersonsController : Controller
{
    const int NUMBER_OF_ENTRIES = 50;

    private readonly IPersonService _personService;
    private readonly ILogger<PersonsController> _logger;
    private readonly ISiteDataService _siteDataService;

    public PersonsController(
        ILogger<PersonsController> logger,
        IPersonService personService,
        ISiteDataService siteDataService)
    {
        _personService = personService;
        _siteDataService = siteDataService;
        _logger = logger;
    }

    public SearchOption GetSearchOption(string? term)
    {
        return term switch {
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
        if (term is null) {
            return "";
        }
        return term;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!_siteDataService.HasAccess()) {
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
        var searchTerm = GetSearchTerm(query["search"]);
        var searchOption = GetSearchOption(query["search_option"]);
        var startIndex = (pageNumber - 1) * NUMBER_OF_ENTRIES;
        var persons = await _personService.FetchPersons(NUMBER_OF_ENTRIES, startIndex, searchTerm, searchOption);
        persons.PageNumber = pageNumber;
        persons.NumberOfPages = (persons.NumberOfEntries / NUMBER_OF_ENTRIES) + 1;
        persons.QueryString = $"&search={searchTerm}";
        persons.SelectedSearchOption = searchOption;
        persons.SearchTerm = searchTerm;
        _logger.LogInformation($"Fetched persons in {stopwatch.Elapsed.TotalMilliseconds} ms");
        return View("Persons", persons);
    }

}
