using RealTimeOpsPortal.Application.Requests.Common;

namespace RealTimeOpsPortal.Application.Requests.GetById;

public sealed class GetServiceRequestByIdUseCase
{
    private readonly IServiceRequestRepository _repository;

    public GetServiceRequestByIdUseCase(IServiceRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceRequestResult?> ExecuteAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        var request =
            await _repository.GetByIdAsync(
                requestId,
                cancellationToken);

        return request?.ToResult();
    }
}