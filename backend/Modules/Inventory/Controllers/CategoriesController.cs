using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Inventory.Controllers;

[Route("api/inventory/categories")]
public class CategoriesController : BaseInventoryController
{
    private readonly CategoryService _categoryService;

    public CategoriesController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var categories = await _categoryService.GetAllCategoriesAsync(orgId, cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var category = await _categoryService.GetCategoryByIdAsync(id, orgId, cancellationToken);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _categoryService.CreateCategoryAsync(orgId, dto, cancellationToken);
        return CreatedAtAction(nameof(GetCategoryById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _categoryService.UpdateCategoryAsync(id, orgId, dto, cancellationToken);
        return Ok(result);
    }
}
