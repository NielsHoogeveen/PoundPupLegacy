using Microsoft.AspNetCore.Mvc;

namespace PoundPupLegacy.Controllers;

[Route("image")]
public class ImageController : Controller
{
    [HttpPost("upload")]
    public async Task<IActionResult> Upload()
    {
        await Task.CompletedTask;
        return new ContentResult {
            Content = @$"{{""url"": ""https://{HttpContext.Request.Host}/files/userimages/image/abondened-child.png""}}",
            ContentType = "text/json"
        };
    }
}

