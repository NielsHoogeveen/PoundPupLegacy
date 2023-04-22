using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.EditModel.UI.Services;
using PoundPupLegacy.ViewModel.UI.Services;

namespace PoundPupLegacy.Controllers;

[Route("image")]
public sealed class ImageController : Controller
{
    private readonly IFetchAttachmentService _attachmentService;
    private readonly IAttachmentStoreService _storeAttachementService;
    public ImageController(
        IFetchAttachmentService attachmentService,
        IAttachmentStoreService storeAttachementService)
    {
        _attachmentService = attachmentService;
        _storeAttachementService = storeAttachementService;
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
        var res = await _storeAttachementService.StoreFile(file);
        if (res is null) {
            return BadRequest();
        }
        return new ContentResult {
            Content = @$"{{""url"": ""https://{HttpContext.Request.Host}/attachment/{res}""}}",
            ContentType = "text/json"
        };
    }
}

