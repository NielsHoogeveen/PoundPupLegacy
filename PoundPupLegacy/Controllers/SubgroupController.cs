using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;

namespace PoundPupLegacy.Controllers;

[Route("subgroup")]
public class SubgroupController : Controller
{
    const int NUMBER_OF_ENTRIES = 150;

    private readonly ISubgroupService _subgroupService;
    private readonly IUserService _userService;
    public SubgroupController(
        ISubgroupService subgroupService,
        IUserService userService)
    {
        _subgroupService = subgroupService;
        _userService = userService;
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
        var userId = _userService.GetUserId(HttpContext.User);
        var page = await _subgroupService.GetSubGroupPagedList(userId, id, NUMBER_OF_ENTRIES, startIndex);
        if (page == null) {
            return NotFound();
        }
        return View("Subgroup", page);
    }
}
