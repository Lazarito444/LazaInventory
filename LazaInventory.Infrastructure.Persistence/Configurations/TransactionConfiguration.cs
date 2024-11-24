using LazaInventory.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LazaInventory.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.HasKey(transaction => transaction.Id);

        builder.Property(transaction => transaction.Id)
            .ValueGeneratedOnAdd();

        builder.Property(transaction => transaction.TransactionType)
            .IsRequired()
            .HasColumnType("NVARCHAR(5)")
            .HasConversion<string>();

        builder.Property(transaction => transaction.Quantity)
            .IsRequired();

        builder.Property(transaction => transaction.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne<Item>(transaction => transaction.Item)
            .WithMany()
            .HasForeignKey(transaction => transaction.ItemId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}