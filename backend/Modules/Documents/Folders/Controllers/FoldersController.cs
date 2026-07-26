using backend.Modules.Documents.Controllers;
using backend.Modules.Documents.Folders.DTOs;
using backend.Modules.Documents.Folders.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Documents.Folders.Controllers;

[Route("api/folders")]
public class FoldersController : BaseDocumentController
{
    private readonly IFolderService _folderService;

    public FoldersController(IFolderService folderService)
    {
        _folderService = folderService;
    }

    [HttpPost]
    public async Task<ActionResult<FolderDto>> Create([FromBody] CreateFolderDto dto)
    {
        var folder = await _folderService.CreateAsync(GetOrganizationId(), dto);
        return Ok(folder);
    }

    [HttpGet]
    public async Task<ActionResult<List<FolderDto>>> GetByParent([FromQuery] Guid? parentFolderId)
    {
        var folders = await _folderService.GetByParentAsync(GetOrganizationId(), parentFolderId);
        return Ok(folders);
    }

    [HttpGet("tree")]
    public async Task<ActionResult<List<FolderTreeDto>>> GetTree()
    {
        var tree = await _folderService.GetTreeAsync(GetOrganizationId());
        return Ok(tree);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FolderDto>> GetById(Guid id)
    {
        var folder = await _folderService.GetByIdAsync(GetOrganizationId(), id);
        return Ok(folder);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FolderDto>> Update(Guid id, [FromBody] UpdateFolderDto dto)
    {
        var updated = await _folderService.UpdateAsync(GetOrganizationId(), id, dto);
        return Ok(updated);
    }

    [HttpPut("{id:guid}/move")]
    public async Task<ActionResult<FolderDto>> Move(Guid id, [FromBody] MoveFolderDto dto)
    {
        var moved = await _folderService.MoveAsync(GetOrganizationId(), id, dto);
        return Ok(moved);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _folderService.DeleteAsync(GetOrganizationId(), id);
        return Ok(new { Message = "Folder deleted successfully." });
    }
}
