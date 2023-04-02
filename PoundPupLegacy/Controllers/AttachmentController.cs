﻿using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;

namespace PoundPupLegacy.Controllers;

[Route("attachment")]
public sealed class AttachmentController : Controller
{
    private readonly IAttachmentService _attachmentService;
    private readonly IUserService _userService;
    private readonly ISiteDataService _siteDataService;
    public AttachmentController(
        IAttachmentService attachmentService,
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
        var tenantId = _siteDataService.GetTenantId(Request);
        var res = await _attachmentService.GetFileStream(id, userId, tenantId);
        return res.Match(
            fr => File(fr.Stream, fr.MimeType, fr.FileName) as IActionResult,
            n => NotFound(),
            e => Problem(e.Value)
        );
    }
}
