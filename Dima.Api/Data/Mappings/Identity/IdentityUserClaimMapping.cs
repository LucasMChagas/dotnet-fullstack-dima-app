using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings.Identity;

public class IdentityUserClaimMapping : IEntityTypeConfiguration<IdentityUserClaim<long>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<long>> builder)
    {
        builder.ToTable("IdentityClaim");
        builder.HasKey(userClaim => userClaim.Id);
        builder.Property(userClaim => userClaim.ClaimType).HasMaxLength(255);
        builder.Property(userClaim => userClaim.ClaimValue).HasMaxLength(255);
    }
}