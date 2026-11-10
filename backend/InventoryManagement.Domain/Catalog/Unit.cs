using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Catalog;

public class Unit : Entity
{
    public string Name { get; private set;} = null!;
    public string Abbreviation {get; private set; } = null!;
    public bool AllowsDecimals { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Unit() { }

    public Unit(string name, string abbreviation, bool allowsDecimals = false)
    {
        Apply(name, abbreviation, allowsDecimals);
    }

    public void Update(string name, string abbreviation, bool allowsDecimals)
    {
        Apply(name, abbreviation, allowsDecimals);
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    private void Apply(string name, string abbreviation, bool allowsDecimals)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Unit name is required.");
        if (name.Trim().Length > 50)
            throw new DomainException("Unit name cannot exceed 50 characters.");
        if (string.IsNullOrWhiteSpace(abbreviation))
            throw new DomainException("Abbreviation is required.");
        if (abbreviation.Trim().Length > 10)
            throw new DomainException("Abbreviation cannot exceed 10 characters.");

        Name = name.Trim();
        Abbreviation = abbreviation.Trim();
        AllowsDecimals = allowsDecimals;
    }
}
