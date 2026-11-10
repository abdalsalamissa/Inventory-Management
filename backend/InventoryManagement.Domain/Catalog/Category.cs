
using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Catalog;

public class Category : Entity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Category() {}

    public Category(string name, string? description = null)
    {
        Apply(name, description);
    }

    public void Update(string name, string? description)
    {
        Apply(name, description);
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    private void Apply(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name is required.");
        if (name.Trim().Length > 100)
            throw new DomainException("Category name cannot exceed 100 characters.");
        if (description is { Length: > 500 })
            throw new DomainException("Description cannot exceed 500 characters.");

        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
    }
}
