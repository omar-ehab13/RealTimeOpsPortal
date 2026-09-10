using RealTimeOpsPortal.Domain.Requests;

namespace RealTimeOpsPortal.Application.Requests.Create;

public sealed class CreateServiceRequestUseCase
{
    private readonly IServiceRequestRepository _repository;

    public CreateServiceRequestUseCase(IServiceRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateServiceRequestResult> ExecuteAsync(
        Guid customerId,
        CreateServiceRequest request,
        CancellationToken cancellationToken = default)
    {
        var serviceRequest = new ServiceRequest(
            customerId,
            request.Description,
            request.Priority);

        await _repository.AddAsync(
            serviceRequest,
            cancellationToken);

        await _repository.SaveChangesAsync(
            cancellationToken);

        return new CreateServiceRequestResult(
            serviceRequest.Id,
            serviceRequest.RequestNumber,
            serviceRequest.Description,
            serviceRequest.Priority,
            serviceRequest.Status,
            serviceRequest.CreatedAtUtc);
    }
}