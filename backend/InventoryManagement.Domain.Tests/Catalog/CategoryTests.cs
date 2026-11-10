using InventoryManagement.Domain.Catalog;
using InventoryManagement.Domain.Common;


namespace InventoryManagement.Domain.Tests.Catalog;

public class CategoryTests
{
    [Fact]
    public void Constructor_ValidInput_TrimsName()
    {
        var category = new Category(" Beverages ");

        Assert.Equal("Beverages", category.Name);
        Assert.True(category.IsActive);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void Constructor_BlackName_Throws(string name)
    {
        Assert.Throws<DomainException>(() => new Category(name));
    }

    [Fact]
    public void Deactivate_ActiveCategory_BecomesInactive()
    {
        var category = new Category("Food");
        category.Deactivate();
        Assert.False(category.IsActive);
    }
}
