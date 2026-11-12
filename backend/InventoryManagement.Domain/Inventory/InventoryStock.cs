using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Inventory;

public class InventoryStock : Entity
{
    public int ProductId { get; private set; }
    public decimal QuantityOnHand { get; private set; }
    public decimal InventoryValue { get; private set; }

    public decimal AverageCost => 
    QuantityOnHand == 0 ? 0 : InventoryValue / QuantityOnHand;

    private InventoryStock() {}

    public InventoryStock(int productId)
    {
        if (productId <= 0) 
            throw new DomainException("ProductId must be greater than 0.");

        ProductId = productId;
        QuantityOnHand = 0;
        InventoryValue = 0;
    }

    public void Receive(decimal quantity, decimal totalCost)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than 0.");
        if (totalCost <= 0)
            throw new DomainException("TotalCost must be greater than 0.");
        if (totalCost != Math.Round(totalCost, 2))
            throw new DomainException("TotalCost must have at most 2 decimal places.");

        QuantityOnHand += quantity;
        InventoryValue += totalCost;
    }

    public decimal Issue(decimal quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than 0.");
        if (quantity > QuantityOnHand)
            throw new DomainException("Insufficient stock.");

        decimal cogs;
        if (quantity == QuantityOnHand)
        {
            cogs = InventoryValue;
        } else
        {
            cogs = Math.Round(quantity * InventoryValue / QuantityOnHand, 2, MidpointRounding.AwayFromZero);
        }

        QuantityOnHand -= quantity;
        InventoryValue -= cogs;

        return cogs;
    }
}
