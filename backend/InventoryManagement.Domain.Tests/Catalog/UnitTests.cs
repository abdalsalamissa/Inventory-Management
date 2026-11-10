using InventoryManagement.Domain.Catalog;
using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Tests.Catalog;

public class UnitTests
{
    [Fact]
    public void Constructor_WithoutAllowsDecimals_DefaultsToFalse()
    {
        // Act
        var unit = new Unit("Piece", "pcs");

        // Assert
        Assert.False(unit.AllowsDecimals);
    }

    [Fact]
    public void Constructor_WithNameOver50Characters_ThrowsDomainException()
    {
        // Arrange
        var longName = new string('a', 51);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            new Unit(longName, "pcs"));

        Assert.Contains("50", exception.Message);
    }
}
