using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;

namespace PoundPupLegacy.Controllers;

[Route("attachment")]
public class AttachmentController : Controller
{
    private readonly IAttachmentService _attachmentService;
    public AttachmentController(IAttachmentService attachmentService)
    {
        _attachmentService = attachmentService; 
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Index(int id)
    {
        var res = await _attachmentService.GetFileStream(id);
        return res.Match(
            fr => File(fr.Stream, fr.MimeType, fr.FileName) as IActionResult,
            n => NotFound(),
            e => Problem(e.Value)
        );
    }
}
