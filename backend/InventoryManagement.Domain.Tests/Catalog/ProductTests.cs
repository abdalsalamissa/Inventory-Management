using InventoryManagement.Domain.Catalog;
using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Tests.Catalog;

public class ProductTests
{
    [Fact]
    public void Constructor_WithValidData_StoresAllValues()
    {
        var name = "Ergonomic Keyboard";
        var categoryId = 5;
        var unitId = 2;
        var description = "Mechanical keyboard with brown switches.";
        var reorderLevel = 15.5m;

        var product = new Product(name, categoryId, unitId, description, reorderLevel);

        Assert.Equal(name, product.Name);
        Assert.Equal(categoryId, product.CategoryId);
        Assert.Equal(unitId, product.UnitId);
        Assert.Equal(description, product.Description);
        Assert.Equal(reorderLevel, product.ReorderLevel);
        Assert.True(product.IsActive);
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(-10.0)]
    public void Constructor_WithNegativeReorderLevel_ThrowsDomainException(decimal reorderLevel)
    {
        var exception = Assert.Throws<DomainException>(() =>
            new Product("Valid Name", 1, 1, null, reorderLevel));

        Assert.Contains("negative", exception.Message);
    }

    [Fact]
    public void Constructor_WithZeroReorderLevel_AcceptsValue()
    {
        var product = new Product("Valid Name", 1, 1, null, 0m);
        Assert.Equal(0m, product.ReorderLevel);
    }

    [Theory]
    [InlineData(0, 1)] // CategoryId is 0
    [InlineData(1, 0)] // UnitId is 0
    [InlineData(0, 0)] // Both are 0
    public void Constructor_WithZeroCategoryIdOrUnitId_ThrowsDomainException(int categoryId, int unitId)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            new Product("Valid Name", categoryId, unitId));
    }

    [Fact]
    public void Update_ChangesAllValues()
    {
        // Arrange
        var product = new Product("Old Name", 1, 1, "Old Description", 5m);
        var newName = "New Name";
        var newCategoryId = 2;
        var newUnitId = 3;
        var newDescription = "New Description";
        var newReorderLevel = 20m;

        // Act
        product.Update(newName, newCategoryId, newUnitId, newDescription, newReorderLevel);

        // Assert
        Assert.Equal(newName, product.Name);
        Assert.Equal(newCategoryId, product.CategoryId);
        Assert.Equal(newUnitId, product.UnitId);
        Assert.Equal(newDescription, product.Description);
        Assert.Equal(newReorderLevel, product.ReorderLevel);
    }
}
