using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;

namespace PoundPupLegacy.Controllers;

[Route("image")]
public sealed class ImageController : Controller
{
    private readonly IAttachmentService _attachmentService;
    public ImageController(IAttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }
    [HttpPost("upload")]
    public async Task<IActionResult> Upload()
    {
        if (!HttpContext.Request.HasFormContentType) {
            return BadRequest();
        }
        if (HttpContext.Request.Form is null) {
            return BadRequest();
        }
        var files = HttpContext.Request.Form.Files;
        if (files.Count != 1) {
            return BadRequest();
        }
        var file = files[0];
        var res = await _attachmentService.StoreFile(file);
        if (res is null) {
            return BadRequest();
        }
        return new ContentResult {
            Content = @$"{{""url"": ""https://{HttpContext.Request.Host}/attachment/{res}""}}",
            ContentType = "text/json"
        };
    }
}

