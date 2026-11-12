using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Inventory;

namespace InventoryManagement.Domain.Tests.Inventory;

public class InventoryStockTests
{
    [Fact]
    public void Constructor_InitializesWithZeroQuantityAndValue()
    {
        var stock = new InventoryStock(1);

        Assert.Equal(1, stock.ProductId);
        Assert.Equal(0m, stock.QuantityOnHand);
        Assert.Equal(0m, stock.InventoryValue);
        Assert.Equal(0m, stock.AverageCost);
    }

    [Fact]
    public void Constructor_WithProductIdZero_Throws()
    {
        Assert.Throws<DomainException>(() => new InventoryStock(0));
    }

    [Fact]
    public void Receive_IncreasesQuantityAndValue()
    {
        var stock = new InventoryStock(1);

        stock.Receive(10m, 100m);

        Assert.Equal(10m, stock.QuantityOnHand);
        Assert.Equal(100m, stock.InventoryValue);
        Assert.Equal(10m, stock.AverageCost);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(-1, 10)]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    public void Receive_WithZeroOrNegativeQuantityOrCost_Throws(decimal quantity, decimal cost)
    {
        var stock = new InventoryStock(1);

        Assert.Throws<DomainException>(() => stock.Receive(quantity, cost));
    }

    [Fact]
    public void Receive_WithCostHavingMoreThanTwoDecimals_Throws()
    {
        var stock = new InventoryStock(1);

        Assert.Throws<DomainException>(() => stock.Receive(3m, 10.005m));
    }

    [Fact]
    public void FailedReceive_LeavesStockUnchanged()
    {
        var stock = new InventoryStock(1);
        stock.Receive(5m, 50m);

        var qtyBefore = stock.QuantityOnHand;
        var valueBefore = stock.InventoryValue;

        Assert.Throws<DomainException>(() => stock.Receive(-1m, 10m));

        Assert.Equal(qtyBefore, stock.QuantityOnHand);
        Assert.Equal(valueBefore, stock.InventoryValue);
    }

    [Fact]
    public void PepsiScenario_ReceiveIssueReceive()
    {
        var stock = new InventoryStock(1);

        stock.Receive(200m, 100m);
        var cogs1 = stock.Issue(100m);
        stock.Receive(300m, 100m);

        Assert.Equal(50m, cogs1);
        Assert.Equal(400m, stock.QuantityOnHand);
        Assert.Equal(150m, stock.InventoryValue);
        Assert.Equal(0.375m, stock.AverageCost);
    }

    [Fact]
    public void ThreeUnitsForTen_IssueOneByOne_RoundingLastUnitAbsorbsPenny()
    {
        var stock = new InventoryStock(1);
        stock.Receive(3m, 10m);

        var cogs1 = stock.Issue(1m);
        var cogs2 = stock.Issue(1m);
        var cogs3 = stock.Issue(1m);

        Assert.Equal(3.33m, cogs1);
        Assert.Equal(3.34m, cogs2);
        Assert.Equal(3.33m, cogs3);
        Assert.Equal(0m, stock.InventoryValue);
        Assert.Equal(0m, stock.QuantityOnHand);

        Assert.Equal(10m, cogs1 + cogs2 + cogs3 + stock.InventoryValue);
    }

    [Fact]
    public void SevenUnitsForTen_IssueTwoTwoThree_RoundingLastUnitAbsorbsRemainder()
    {
        var stock = new InventoryStock(1);
        stock.Receive(7m, 10m);

        var cogs1 = stock.Issue(2m);
        var cogs2 = stock.Issue(2m);
        var cogs3 = stock.Issue(3m);

        Assert.Equal(2.86m, cogs1);
        Assert.Equal(2.86m, cogs2);
        Assert.Equal(4.28m, cogs3);
        Assert.Equal(0m, stock.QuantityOnHand);
        Assert.Equal(0m, stock.InventoryValue);

        Assert.Equal(10m, cogs1 + cogs2 + cogs3 + stock.InventoryValue);
    }

    [Fact]
    public void SevenUnitsForTen_IssueTwoTwoTwoOne_RoundingLastUnitAbsorbsRemainder()
    {
        var stock = new InventoryStock(1);
        stock.Receive(7m, 10m);

        var cogs1 = stock.Issue(2m);
        var cogs2 = stock.Issue(2m);
        var cogs3 = stock.Issue(2m);
        var cogs4 = stock.Issue(1m);

        Assert.Equal(2.86m, cogs1);
        Assert.Equal(2.86m, cogs2);
        Assert.Equal(2.85m, cogs3);
        Assert.Equal(1.43m, cogs4);
        Assert.Equal(0m, stock.QuantityOnHand);
        Assert.Equal(0m, stock.InventoryValue);

        Assert.Equal(10m, cogs1 + cogs2 + cogs3 + cogs4 + stock.InventoryValue);
    }

    [Fact]
    public void Issue_MoreThanOnHand_Throws()
    {
        var stock = new InventoryStock(1);
        stock.Receive(5m, 50m);

        Assert.Throws<DomainException>(() => stock.Issue(10m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Issue_WithZeroOrNegativeQuantity_Throws(decimal quantity)
    {
        var stock = new InventoryStock(1);
        stock.Receive(5m, 50m);

        Assert.Throws<DomainException>(() => stock.Issue(quantity));
    }

    [Fact]
    public void Issue_FromEmptyStock_Throws()
    {
        var stock = new InventoryStock(1);

        Assert.Throws<DomainException>(() => stock.Issue(1m));
    }

    [Fact]
    public void FailedIssue_LeavesStockUnchanged()
    {
        var stock = new InventoryStock(1);
        stock.Receive(5m, 50m);

        var qtyBefore = stock.QuantityOnHand;
        var valueBefore = stock.InventoryValue;

        Assert.Throws<DomainException>(() => stock.Issue(10m));

        Assert.Equal(qtyBefore, stock.QuantityOnHand);
        Assert.Equal(valueBefore, stock.InventoryValue);
    }
}