using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings;

public class OrderMapping : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        
        builder.HasKey(order => order.Id);
        
        builder.Property(order => order.Number)
            .IsRequired(true)
            .HasColumnType("CHAR")
            .HasMaxLength(8);
        
        builder.Property(order => order.ExternalReference)
            .IsRequired(false)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(64);
        
        builder.Property(order => order.Gateway)
            .IsRequired(true)
            .HasColumnType("SMALLINT");
        
        builder.Property(order => order.CreatedAt)
            .IsRequired(true)
            .HasColumnType("DATETIME2");
        
        builder.Property(order => order.UpdatedAt)
            .IsRequired(true)
            .HasColumnType("DATETIME2");
        
        builder.Property(order => order.Status)
            .IsRequired(true)
            .HasColumnType("SMALLINT");
        
        builder.Property(order => order.UserId)
            .IsRequired(false)
            .HasColumnType("VARCHAR")
            .HasMaxLength(160);

        builder.HasOne(order => order.Product).WithMany();
        builder.HasOne(order => order.Voucher).WithMany();
        

    }
}