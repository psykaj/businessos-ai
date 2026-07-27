using backend.Common;

namespace backend.Modules.Inventory.Entities;

public class ProductCategory : BaseEntity
{
    public Guid OrganizationId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public Guid? ParentCategoryId { get; set; }
    public ProductCategory? ParentCategory { get; set; }
    
    public ICollection<ProductCategory> SubCategories { get; set; } = new List<ProductCategory>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
