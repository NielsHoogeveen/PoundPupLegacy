using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;

namespace PoundPupLegacy.Controllers;

[Route("subgroup")]
public class SubgroupController : Controller
{
    const int NUMBER_OF_ENTRIES = 150;

    private readonly ISubgroupService _subgroupService;
    public SubgroupController(ISubgroupService subgroupService)
    {
        _subgroupService = subgroupService; 
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Index(int id)
    {
        var pageNumber = 1;

        var query = this.HttpContext.Request.Query;
        var pageValue = query["page"];
        if (!string.IsNullOrEmpty(pageValue)) {
            if (int.TryParse(pageValue, out int providedPageNumber)) {
                pageNumber = providedPageNumber;
            }
        }
        var startIndex = (pageNumber - 1) * NUMBER_OF_ENTRIES;
        var page = await _subgroupService.GetSubGroupPagedList(id, NUMBER_OF_ENTRIES, startIndex);
        if(page == null) {
            return NotFound();
       }
        return View("Subgroup", page);
    }
}
