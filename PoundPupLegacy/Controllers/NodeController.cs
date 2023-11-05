using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;

namespace PoundPupLegacy.Controllers;

public class NodeController(INodeService nodeService, ISiteDataService siteDataService) : Controller
{
    [HttpGet("/node/{id}")]
    public async Task<IActionResult> Index(int id)
    {
        var path = await nodeService.GetRedirectPath(id, siteDataService.GetTenant().Id);
        if (path is not null) {
            return RedirectPermanentPreserveMethod(path);
        }
        else {
            return NotFound();
        }
    }
}
