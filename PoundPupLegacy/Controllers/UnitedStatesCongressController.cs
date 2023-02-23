using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;

namespace PoundPupLegacy.Controllers;
[Route("united_states_congress")]
public class UnitedStatesCongressController : Controller {

    private readonly ICongressionalDataService _congressionalDataService;
    public UnitedStatesCongressController(ICongressionalDataService congressionalDataService) 
    { 
        _congressionalDataService = congressionalDataService;
    }
    public async Task<IActionResult> Index() {
        var html = await _congressionalDataService.GetUnitedStatesCongress();
        if(html is null) {
            return NotFound();
        }
        return new ContentResult {
            Content = html,
            ContentType = "text/html",
        };
    }
}
