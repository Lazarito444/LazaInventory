using LazaInventory.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LazaInventory.Infrastructure.Persistence.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .ValueGeneratedOnAdd();

        builder.Property(item => item.Name)
            .IsRequired()
            .HasColumnType("NVARCHAR(100)");

        builder.Property(item => item.Price)
            .IsRequired()
            .HasColumnType("DECIMAL(10,2)");

        builder.Property(item => item.Stock)
            .HasDefaultValue(0);

        builder.Property(item => item.MinimumStock)
            .HasDefaultValue(0);

        builder.Property(item => item.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne<Category>(item => item.Category)
            .WithMany(category => category.Items)
            .HasForeignKey(item => item.CategoryId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}