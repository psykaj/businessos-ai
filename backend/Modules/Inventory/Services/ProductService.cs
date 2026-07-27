using AutoMapper;
using backend.Common;
using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Helpers;
using backend.Modules.Inventory.Interfaces;

namespace backend.Modules.Inventory.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IMapper _mapper;
    private readonly InventoryCacheService _cacheService;

    public ProductService(
        IProductRepository productRepository,
        IWarehouseRepository warehouseRepository,
        IStockRepository stockRepository,
        IMapper mapper,
        InventoryCacheService cacheService)
    {
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _stockRepository = stockRepository;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<ProductResponseDto> CreateProductAsync(Guid organizationId, CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        var existingSKU = await _productRepository.GetBySKUAsync(dto.SKU, organizationId, cancellationToken);
        if (existingSKU != null) throw new InvalidOperationException($"Product with SKU '{dto.SKU}' already exists.");

        var product = new Product
        {
            OrganizationId = organizationId,
            SKU = dto.SKU.Trim().ToUpper(),
            Name = dto.Name.Trim(),
            Description = dto.Description,
            Barcode = dto.Barcode,
            CategoryId = dto.CategoryId,
            UnitOfMeasure = dto.UnitOfMeasure,
            CostPrice = dto.CostPrice,
            SellingPrice = dto.SellingPrice,
            ReorderPoint = dto.ReorderPoint,
            SafetyStock = dto.SafetyStock,
            ReorderQuantity = dto.ReorderQuantity,
            MinimumStockLevel = dto.MinimumStockLevel,
            MaxStockLevel = dto.MaxStockLevel,
            IsActive = true,
            IsArchived = false
        };

        product.QRCode = string.IsNullOrEmpty(dto.QRCode) 
            ? BarcodeQrHelper.GenerateProductQrCode(organizationId, product.Id, product.SKU)
            : dto.QRCode;

        await _productRepository.AddAsync(product, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);

        // Auto-initialize inventory stock record in primary warehouse if exists
        var primaryWarehouse = await _warehouseRepository.GetPrimaryAsync(organizationId, cancellationToken);
        if (primaryWarehouse != null)
        {
            var initialStock = new InventoryStock
            {
                OrganizationId = organizationId,
                ProductId = product.Id,
                WarehouseId = primaryWarehouse.Id,
                QuantityOnHand = 0,
                QuantityReserved = 0,
                ReorderLevel = product.ReorderPoint
            };
            await _stockRepository.AddAsync(initialStock, cancellationToken);
            await _stockRepository.SaveChangesAsync(cancellationToken);
        }

        var created = await _productRepository.GetByIdAsync(product.Id, organizationId, cancellationToken);
        var result = _mapper.Map<ProductResponseDto>(created!);
        
        await _cacheService.SetAsync($"product:{organizationId}:{product.Id}", result, TimeSpan.FromMinutes(10), cancellationToken);
        return result;
    }

    public async Task<ProductResponseDto> UpdateProductAsync(Guid id, Guid organizationId, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, organizationId, cancellationToken);
        if (product == null) throw new KeyNotFoundException($"Product {id} not found.");

        product.Name = dto.Name.Trim();
        product.Description = dto.Description;
        product.Barcode = dto.Barcode;
        if (!string.IsNullOrEmpty(dto.QRCode)) product.QRCode = dto.QRCode;
        product.CategoryId = dto.CategoryId;
        product.UnitOfMeasure = dto.UnitOfMeasure;
        product.CostPrice = dto.CostPrice;
        product.SellingPrice = dto.SellingPrice;
        product.ReorderPoint = dto.ReorderPoint;
        product.SafetyStock = dto.SafetyStock;
        product.ReorderQuantity = dto.ReorderQuantity;
        product.MinimumStockLevel = dto.MinimumStockLevel;
        product.MaxStockLevel = dto.MaxStockLevel;
        product.IsActive = dto.IsActive;

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync(cancellationToken);

        var updated = await _productRepository.GetByIdAsync(id, organizationId, cancellationToken);
        var result = _mapper.Map<ProductResponseDto>(updated!);

        await _cacheService.RemoveAsync($"product:{organizationId}:{id}", cancellationToken);
        await _cacheService.SetAsync($"product:{organizationId}:{id}", result, TimeSpan.FromMinutes(10), cancellationToken);

        return result;
    }

    public async Task<bool> ArchiveProductAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, organizationId, cancellationToken);
        if (product == null) return false;

        product.IsArchived = true;
        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync($"product:{organizationId}:{id}", cancellationToken);
        return true;
    }

    public async Task<ProductResponseDto?> GetProductByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var cached = await _cacheService.GetAsync<ProductResponseDto>($"product:{organizationId}:{id}", cancellationToken);
        if (cached != null) return cached;

        var product = await _productRepository.GetByIdAsync(id, organizationId, cancellationToken);
        if (product == null) return null;

        var result = _mapper.Map<ProductResponseDto>(product);
        await _cacheService.SetAsync($"product:{organizationId}:{id}", result, TimeSpan.FromMinutes(10), cancellationToken);
        return result;
    }

    public async Task<ProductResponseDto?> GetProductBySKUAsync(string sku, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetBySKUAsync(sku, organizationId, cancellationToken);
        return product != null ? _mapper.Map<ProductResponseDto>(product) : null;
    }

    public async Task<BarcodeLookupDto> LookupBarcodeAsync(string code, Guid organizationId, CancellationToken cancellationToken = default)
    {
        // Try QR code payload parsing first
        if (BarcodeQrHelper.TryParseQrCode(code, out var productId, out var sku))
        {
            var pByQr = await _productRepository.GetByIdAsync(productId, organizationId, cancellationToken);
            if (pByQr != null) return new BarcodeLookupDto(code, _mapper.Map<ProductResponseDto>(pByQr));
        }

        // Direct barcode/QR property match
        var product = await _productRepository.GetByBarcodeAsync(code, organizationId, cancellationToken);
        if (product != null) return new BarcodeLookupDto(code, _mapper.Map<ProductResponseDto>(product));

        // SKU fallback match
        var productBySku = await _productRepository.GetBySKUAsync(code, organizationId, cancellationToken);
        if (productBySku != null) return new BarcodeLookupDto(code, _mapper.Map<ProductResponseDto>(productBySku));

        return new BarcodeLookupDto(code, null);
    }

    public async Task<PagedResult<ProductResponseDto>> SearchProductsAsync(Guid organizationId, ProductSearchFilterDto filter, CancellationToken cancellationToken = default)
    {
        var pagedProducts = await _productRepository.GetPagedAsync(organizationId, filter, cancellationToken);
        return new PagedResult<ProductResponseDto>(
            _mapper.Map<List<ProductResponseDto>>(pagedProducts.Items),
            pagedProducts.TotalCount,
            pagedProducts.PageNumber,
            pagedProducts.PageSize
        );
    }
}
