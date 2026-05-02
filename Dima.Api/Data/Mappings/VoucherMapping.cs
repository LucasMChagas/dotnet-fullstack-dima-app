using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings;

public class VoucherMapping : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.ToTable("Voucher");
        
        builder.HasKey(voucher => voucher.Id );
        
        builder.Property(voucher => voucher.Number)
            .IsRequired(true)
            .HasColumnType("CHAR")
            .HasMaxLength(8);
        
        builder.Property(voucher => voucher.Title)
            .IsRequired(true)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);
        
        builder.Property(voucher => voucher.Description)
            .IsRequired(false)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(255);
        
        builder.Property(voucher => voucher.Amount)
            .IsRequired(true)
            .HasColumnType("MONEY");
        
        builder.Property(voucher => voucher.IsActive)
            .IsRequired(true)
            .HasColumnType("BIT");
    }
}