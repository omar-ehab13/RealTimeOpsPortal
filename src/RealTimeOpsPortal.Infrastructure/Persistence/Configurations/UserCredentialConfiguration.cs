using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeOpsPortal.Domain.Users;
using RealTimeOpsPortal.Infrastructure.Persistence.Authentication;

namespace RealTimeOpsPortal.Infrastructure.Persistence.Configurations;

public class UserCredentialConfiguration
    : IEntityTypeConfiguration<UserCredential>
{
    public void Configure(
        EntityTypeBuilder<UserCredential> builder)
    {
        builder.ToTable("UserCredentials");

        builder.HasKey(x => x.UserId);

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<UserCredential>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}