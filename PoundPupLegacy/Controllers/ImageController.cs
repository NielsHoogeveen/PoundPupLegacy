using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.EditModel.UI.Services;
using PoundPupLegacy.ViewModel.UI.Services;

namespace PoundPupLegacy.Controllers;

[Route("image")]
public sealed class ImageController(
        ILogger<ImageController> logger,
        IAttachmentStoreService storeAttachementService) : Controller
{
    [HttpPost("upload")]
    public async Task<IActionResult> Upload()
    {
        if (!HttpContext.Request.HasFormContentType) {
            logger.LogInformation("Form has no content type in upload");
            return BadRequest();
        }
        if (HttpContext.Request.Form is null) {
            logger.LogInformation("Form is empty in upload");
            return BadRequest();
        }
        var files = HttpContext.Request.Form.Files;
        if (files.Count != 1) {
            logger.LogInformation("Form does not contain one file in upload");
            return BadRequest();
        }
        var file = files[0];
        var res = await storeAttachementService.StoreFile(file);
        if (res is null) {
            logger.LogInformation("Form does not contain one file in upload");
            return BadRequest();
        }
        return new ContentResult {
            Content = @$"{{""url"": ""https://{HttpContext.Request.Host}/attachment/{res}""}}",
            ContentType = "text/json"
        };
    }
}

