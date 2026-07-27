using AutoMapper;
using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Entities;

namespace backend.Modules.Inventory.Mappings;

public class InventoryMappingProfile : Profile
{
    public InventoryMappingProfile()
    {
        CreateMap<Product, ProductResponseDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ForMember(dest => dest.TotalQuantityOnHand, opt => opt.MapFrom(src => src.StockLevels.Sum(s => s.QuantityOnHand)))
            .ForMember(dest => dest.TotalQuantityReserved, opt => opt.MapFrom(src => src.StockLevels.Sum(s => s.QuantityReserved)))
            .ForMember(dest => dest.TotalQuantityAvailable, opt => opt.MapFrom(src => src.StockLevels.Sum(s => s.QuantityOnHand - s.QuantityReserved)));

        CreateMap<ProductCategory, CategoryResponseDto>()
            .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null))
            .ForMember(dest => dest.ProductsCount, opt => opt.MapFrom(src => src.Products.Count));

        CreateMap<Warehouse, WarehouseResponseDto>()
            .ForMember(dest => dest.TotalItemsCount, opt => opt.MapFrom(src => src.StockItems.Count));

        CreateMap<InventoryStock, InventoryStockResponseDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.ProductSKU, opt => opt.MapFrom(src => src.Product != null ? src.Product.SKU : string.Empty))
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.Name : string.Empty));

        CreateMap<StockMovement, StockMovementResponseDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.ProductSKU, opt => opt.MapFrom(src => src.Product != null ? src.Product.SKU : string.Empty))
            .ForMember(dest => dest.SourceWarehouseName, opt => opt.MapFrom(src => src.SourceWarehouse != null ? src.SourceWarehouse.Name : null))
            .ForMember(dest => dest.DestinationWarehouseName, opt => opt.MapFrom(src => src.DestinationWarehouse != null ? src.DestinationWarehouse.Name : null));

        CreateMap<Supplier, SupplierResponseDto>();

        CreateMap<PurchaseOrder, PurchaseOrderResponseDto>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : string.Empty))
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.Name : string.Empty));

        CreateMap<PurchaseOrderItem, PurchaseOrderItemResponseDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.ProductSKU, opt => opt.MapFrom(src => src.Product != null ? src.Product.SKU : string.Empty));

        CreateMap<GoodsReceipt, GoodsReceiptResponseDto>()
            .ForMember(dest => dest.PONumber, opt => opt.MapFrom(src => src.PurchaseOrder != null ? src.PurchaseOrder.PONumber : string.Empty))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : string.Empty))
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.Name : string.Empty));

        CreateMap<GoodsReceiptItem, GoodsReceiptItemResponseDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.ProductSKU, opt => opt.MapFrom(src => src.Product != null ? src.Product.SKU : string.Empty));

        CreateMap<StockAdjustment, StockAdjustmentResponseDto>()
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.Name : string.Empty));

        CreateMap<StockAdjustmentItem, StockAdjustmentItemResponseDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.ProductSKU, opt => opt.MapFrom(src => src.Product != null ? src.Product.SKU : string.Empty));
    }
}
