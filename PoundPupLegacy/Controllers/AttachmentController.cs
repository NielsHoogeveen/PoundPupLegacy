using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;
using PoundPupLegacy.ViewModel.UI.Services;

namespace PoundPupLegacy.Controllers;

[Route("attachment")]
public sealed class AttachmentController : Controller
{
    private readonly IFetchAttachmentService _attachmentService;
    private readonly IUserService _userService;
    private readonly ISiteDataService _siteDataService;
    public AttachmentController(
        IFetchAttachmentService attachmentService,
        ISiteDataService siteDataService,
        IUserService userService)
    {
        _attachmentService = attachmentService;
        _userService = userService;
        _siteDataService = siteDataService;
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Index(int id)
    {
        var userId = _userService.GetUserId(HttpContext.User);
        var tenantId = _siteDataService.GetTenantId(Request.GetUri());
        var res = await _attachmentService.GetFileStream(
            id: id,
            userId: userId,
            tenantId: tenantId);
        return res.Match(
            fr => File(fr.Stream, fr.MimeType, fr.FileName) as IActionResult,
            n => NotFound(),
            e => Problem(e.Value)
        );
    }
}
