using backend.Modules.Documents.Controllers;
using backend.Modules.Documents.Documents.DTOs;
using backend.Modules.Documents.Documents.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Documents.Documents.Controllers;

[Route("api/documents")]
public class DocumentsController : BaseDocumentController
{
    private readonly IDocumentService _documentService;

    public DocumentsController(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<DocumentDto>> Upload([FromForm] IFormFile file, [FromForm] UploadDocumentDto dto)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file was uploaded.");
        }

        using var stream = file.OpenReadStream();
        var result = await _documentService.UploadAsync(
            GetOrganizationId(),
            GetUserId(),
            GetUserName(),
            stream,
            file.FileName,
            file.ContentType,
            dto
        );

        return Ok(result);
    }

    [HttpPost("{id:guid}/versions")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<DocumentDto>> UploadNewVersion(Guid id, [FromForm] IFormFile file, [FromForm] string? changesSummary)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file was uploaded.");
        }

        using var stream = file.OpenReadStream();
        var result = await _documentService.UploadNewVersionAsync(
            GetOrganizationId(),
            id,
            GetUserId(),
            GetUserName(),
            stream,
            file.FileName,
            file.ContentType,
            changesSummary
        );

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DocumentDto>> GetById(Guid id)
    {
        var document = await _documentService.GetByIdAsync(GetOrganizationId(), id);
        return Ok(document);
    }

    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id, [FromQuery] Guid? versionId)
    {
        var (stream, contentType, fileName) = await _documentService.DownloadAsync(GetOrganizationId(), id, versionId);
        return File(stream, contentType, fileName);
    }

    [HttpGet("{id:guid}/preview")]
    public async Task<ActionResult<DocumentPreviewDto>> GetPreview(Guid id)
    {
        var preview = await _documentService.GetPreviewAsync(GetOrganizationId(), id);
        return Ok(preview);
    }

    [HttpPut("{id:guid}/rename")]
    public async Task<ActionResult<DocumentDto>> Rename(Guid id, [FromBody] RenameDocumentDto dto)
    {
        var updated = await _documentService.RenameAsync(GetOrganizationId(), id, dto);
        return Ok(updated);
    }

    [HttpPut("{id:guid}/move")]
    public async Task<ActionResult<DocumentDto>> Move(Guid id, [FromBody] MoveDocumentDto dto)
    {
        var updated = await _documentService.MoveAsync(GetOrganizationId(), id, dto);
        return Ok(updated);
    }

    [HttpPost("{id:guid}/copy")]
    public async Task<ActionResult<DocumentDto>> Copy(Guid id, [FromBody] CopyDocumentDto dto)
    {
        var copied = await _documentService.CopyAsync(GetOrganizationId(), id, dto);
        return Ok(copied);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        await _documentService.SoftDeleteAsync(GetOrganizationId(), id, GetUserId(), GetUserName());
        return Ok(new { Message = "Document soft deleted successfully." });
    }

    [HttpDelete("{id:guid}/permanent")]
    public async Task<IActionResult> PermanentDelete(Guid id)
    {
        await _documentService.PermanentDeleteAsync(GetOrganizationId(), id);
        return Ok(new { Message = "Document permanently deleted." });
    }

    [HttpPost("{id:guid}/restore")]
    public async Task<ActionResult<DocumentDto>> Restore(Guid id)
    {
        var restored = await _documentService.RestoreAsync(GetOrganizationId(), id);
        return Ok(restored);
    }

    [HttpGet("{id:guid}/versions")]
    public async Task<ActionResult<List<DocumentVersionDto>>> GetVersions(Guid id)
    {
        var versions = await _documentService.GetVersionsAsync(GetOrganizationId(), id);
        return Ok(versions);
    }

    [HttpPost("{id:guid}/versions/{versionId:guid}/revert")]
    public async Task<ActionResult<DocumentDto>> RevertToVersion(Guid id, Guid versionId)
    {
        var reverted = await _documentService.RevertToVersionAsync(GetOrganizationId(), id, versionId, GetUserId(), GetUserName());
        return Ok(reverted);
    }

    [HttpGet("search")]
    public async Task<ActionResult> Search([FromQuery] DocumentSearchQueryDto query)
    {
        var (items, totalCount) = await _documentService.SearchAsync(GetOrganizationId(), query);
        return Ok(new { Items = items, TotalCount = totalCount, Page = query.Page, PageSize = query.PageSize });
    }

    [HttpPut("{id:guid}/tags")]
    public async Task<ActionResult<DocumentDto>> UpdateTags(Guid id, [FromBody] UpdateTagsDto dto)
    {
        var updated = await _documentService.UpdateTagsAsync(GetOrganizationId(), id, dto);
        return Ok(updated);
    }

    [HttpPost("{id:guid}/favorite")]
    public async Task<ActionResult<DocumentDto>> ToggleFavorite(Guid id)
    {
        var updated = await _documentService.ToggleFavoriteAsync(GetOrganizationId(), id);
        return Ok(updated);
    }

    [HttpGet("favorites")]
    public async Task<ActionResult<List<DocumentDto>>> GetFavorites()
    {
        var favorites = await _documentService.GetFavoritesAsync(GetOrganizationId());
        return Ok(favorites);
    }

    [HttpPost("test-suite")]
    public async Task<IActionResult> RunTestSuite()
    {
        await Tests.DocumentManagementSystemTests.RunAllTestsAsync();
        return Ok(new { Message = "All Document Management System integration tests passed successfully!" });
    }
}
