using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeOpsPortal.Domain.Requests;
using RealTimeOpsPortal.Domain.Users;

namespace RealTimeOpsPortal.Infrastructure.Persistence.Configurations;

public class RequestStatusHistoryConfiguration
    : IEntityTypeConfiguration<RequestStatusHistory>
{
    public void Configure(
        EntityTypeBuilder<RequestStatusHistory> builder)
    {
        builder.ToTable("RequestStatusHistories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ToStatus)
            .IsRequired();

        builder.Property(x => x.ChangedAtUtc)
            .IsRequired();

        builder.HasOne<ServiceRequest>()
            .WithMany(x => x.StatusHistory)
            .HasForeignKey(x => x.ServiceRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}