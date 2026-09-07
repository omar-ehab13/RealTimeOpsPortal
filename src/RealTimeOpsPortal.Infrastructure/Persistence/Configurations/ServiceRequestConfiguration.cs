using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeOpsPortal.Domain.Requests;
using RealTimeOpsPortal.Domain.Users;

namespace RealTimeOpsPortal.Infrastructure.Persistence.Configurations;

public class ServiceRequestConfiguration
    : IEntityTypeConfiguration<ServiceRequest>
{
    public void Configure(
        EntityTypeBuilder<ServiceRequest> builder)
    {
        builder.ToTable("ServiceRequests");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RequestNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.RequestNumber)
            .IsUnique();

        builder.Property(x => x.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(x => x.Priority)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.AssignedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property<byte[]>("RowVersion")
            .IsRowVersion()
            .IsConcurrencyToken();

        builder.Navigation(x => x.StatusHistory)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}