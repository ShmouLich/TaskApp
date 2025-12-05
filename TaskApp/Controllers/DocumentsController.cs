using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskApp.Services.Interfaces;

namespace TaskApp.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _documentService;

    public DocumentsController(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> Download(int id)
    {
        var document = await _documentService.GetDocumentByIdAsync(id);

        if (document == null)
        {
            return NotFound();
        }

        return File(document.FileData, document.ContentType, document.FileName);
    }
}
