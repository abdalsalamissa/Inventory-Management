using InventoryManagement.Domain.Catalog;
using InventoryManagement.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Persistence.Configurations;

public class InventoryStockConfiguration : IEntityTypeConfiguration<InventoryStock>
{
    public void Configure(EntityTypeBuilder<InventoryStock> builder)
    {
        builder.ToTable("InventoryStocks", t =>
        {
            t.HasCheckConstraint(
                "CK_InventoryStocks_QuantityOnHand_NonNegative",
                "[QuantityOnHand] >= 0");

            t.HasCheckConstraint(
                "CK_InventoryStocks_InventoryValue_NonNegative",
                "[InventoryValue] >= 0");
            
            t.HasCheckConstraint(
                "CK_InventoryStocks_QuantityOrValue_ZeroTogether",
                "[QuantityOnHand] > 0 OR [InventoryValue] = 0");
        });

        builder.Property(s => s.QuantityOnHand).HasColumnType("decimal(18,3)");
        builder.Property(s => s.InventoryValue).HasColumnType("decimal(18,2)");

        builder.HasIndex(s => s.ProductId).IsUnique();

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Ignore(s => s.AverageCost);

        builder.Property<byte[]>("RowVersion").IsRowVersion();
    }
}
