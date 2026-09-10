using RealTimeOpsPortal.Application.Requests.Common;

namespace RealTimeOpsPortal.Application.Requests.GetMy;

public sealed class GetMyServiceRequestsUseCase
{
    private readonly IServiceRequestRepository _repository;

    public GetMyServiceRequestsUseCase(
        IServiceRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ServiceRequestResult>> ExecuteAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var requests =
            await _repository.GetByCustomerIdAsync(
                customerId,
                cancellationToken);

        return requests
            .Select(x => x.ToResult())
            .ToList();
    }
}