using InventoryManagement.Domain.Common;


namespace InventoryManagement.Domain.Catalog;

public class Product : Entity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public int CategoryId { get; private set; }
    public int UnitId { get; private set; }
    public decimal? ReorderLevel { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Product() { }

    public Product(string name, int categoryId, int unitId, string? description = null, decimal? reorderLevel = null)
    {
        Apply(name, categoryId, unitId, description, reorderLevel);
    }

    public void Update(string name, int categoryId, int unitId, string? description, decimal? reorderLevel)
    {
        Apply(name, categoryId, unitId, description, reorderLevel);
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    private void Apply(string name, int categoryId, int unitId, string? description, decimal? reorderLevel)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name is required.");
        if (name.Trim().Length > 200)
            throw new DomainException("Product name cannot exceed 200 characters.");
        if (categoryId <= 0)
            throw new DomainException("CategoryId must be greater than 0.");
        if (unitId <= 0)
            throw new DomainException("UnitId must be greater than 0.");
        if (description is { Length: > 1000})
            throw new DomainException("Description cannot exceed 1000 characters.");
        if (reorderLevel.HasValue && reorderLevel.Value < 0)
            throw new DomainException("ReorderLevel must not be negative.");
        
        Name = name.Trim();
        CategoryId = categoryId;
        UnitId = unitId;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        ReorderLevel = reorderLevel;
    }
}
