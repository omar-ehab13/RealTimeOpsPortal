using Microsoft.EntityFrameworkCore;
using RealTimeOpsPortal.Application.Requests;
using RealTimeOpsPortal.Domain.Requests;

namespace RealTimeOpsPortal.Infrastructure.Persistence.Repositories;

public sealed class ServiceRequestRepository : IServiceRequestRepository
{
    private readonly AppDbContext _context;

    public ServiceRequestRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ServiceRequest serviceRequest, CancellationToken cancellationToken = default)
    {
        await _context.ServiceRequests.AddAsync(
            serviceRequest,
            cancellationToken);
    }

    public Task<ServiceRequest?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _context.ServiceRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<ServiceRequest>>
        GetByCustomerIdAsync(
            Guid customerId,
            CancellationToken cancellationToken = default)
    {
        return await _context.ServiceRequests
            .AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        ServiceRequest serviceRequest,
        CancellationToken cancellationToken = default)
    {
        _context.ServiceRequests.Update(serviceRequest);

        try
        {
            await SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Handle concurrency conflict: another user modified the request simultaneously
            // Log the conflict and re-throw with meaningful context
            throw new InvalidOperationException(
                $"The request (ID: {serviceRequest.Id}) was modified by another user. Please refresh and try again.",
                ex);
        }
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(
            cancellationToken);
    }
}
