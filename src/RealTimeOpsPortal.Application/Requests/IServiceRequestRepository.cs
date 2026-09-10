using RealTimeOpsPortal.Domain.Requests;

namespace RealTimeOpsPortal.Application.Requests;

public interface IServiceRequestRepository
{
    Task AddAsync(
        ServiceRequest serviceRequest,
        CancellationToken cancellationToken = default);

    Task<ServiceRequest?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ServiceRequest>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        ServiceRequest serviceRequest,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
