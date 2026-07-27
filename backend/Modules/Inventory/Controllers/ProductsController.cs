using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Inventory.Controllers;

[Route("api/inventory/products")]
public class ProductsController : BaseInventoryController
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> SearchProducts([FromQuery] ProductSearchFilterDto filter, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _productService.SearchProductsAsync(orgId, filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var product = await _productService.GetProductByIdAsync(id, orgId, cancellationToken);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpGet("sku/{sku}")]
    public async Task<IActionResult> GetProductBySKU(string sku, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var product = await _productService.GetProductBySKUAsync(sku, orgId, cancellationToken);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpGet("barcode/{code}")]
    public async Task<IActionResult> LookupBarcode(string code, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _productService.LookupBarcodeAsync(code, orgId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _productService.CreateProductAsync(orgId, dto, cancellationToken);
        return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto dto, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _productService.UpdateProductAsync(id, orgId, dto, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/archive")]
    public async Task<IActionResult> ArchiveProduct(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var success = await _productService.ArchiveProductAsync(id, orgId, cancellationToken);
        if (!success) return NotFound();
        return NoContent();
    }
}
