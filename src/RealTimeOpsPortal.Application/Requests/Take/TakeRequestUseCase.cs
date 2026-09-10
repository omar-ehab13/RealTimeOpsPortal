using Microsoft.Extensions.Logging;
using RealTimeOpsPortal.Application.Notifications;
using RealTimeOpsPortal.Application.Requests;
using RealTimeOpsPortal.Domain.Requests;

namespace RealTimeOpsPortal.Application.Requests.Take;

/// <summary>
/// Use case for taking (assigning to self) a service request.
/// </summary>
public class TakeRequestUseCase
{
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IRequestNotificationService _notificationService;

    public TakeRequestUseCase(
        IServiceRequestRepository serviceRequestRepository,
        IRequestNotificationService notificationService)
    {
        _serviceRequestRepository = serviceRequestRepository;
        _notificationService = notificationService;
    }

    /// <summary>
    /// Executes the take request operation.
    /// </summary>
    /// <param name="command">The take request command containing RequestId and AgentId.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result of the take request operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the service request is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when request is already assigned or concurrent update occurred.</exception>
    public async Task<TakeRequestResult> ExecuteAsync(
        TakeRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        // Validate command
        if (command.RequestId == Guid.Empty)
            throw new ArgumentException("Request ID is required.", nameof(command.RequestId));

        if (command.AgentId == Guid.Empty)
            throw new ArgumentException("Agent ID is required.", nameof(command.AgentId));

        // Get the service request
        var serviceRequest = await _serviceRequestRepository.GetByIdAsync(
            command.RequestId,
            cancellationToken);

        if (serviceRequest == null)
        {
            throw new KeyNotFoundException(
                $"Service request with ID '{command.RequestId}' not found.");
        }

        // Attempt to take the request
        // This will throw InvalidOperationException if already assigned
        try
        {
            serviceRequest.Take(command.AgentId);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("already assigned"))
        {
            throw new InvalidOperationException(
                $"The request (ID: {command.RequestId}) is already assigned to another agent.",
                ex);
        }

        // Update in database with concurrency handling
        // If concurrent conflict occurs, InvalidOperationException will be thrown by repository
        try
        {
            await _serviceRequestRepository.UpdateAsync(
                serviceRequest,
                cancellationToken);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("modified by another user"))
        {
            throw;
        }

        await _notificationService
    .NotifyRequestAssignedAsync(
        new RequestAssignedNotification(
            command.RequestId,
            serviceRequest.RequestNumber,
            serviceRequest.CustomerId,
            command.AgentId,
            DateTime.UtcNow),
        cancellationToken);

        // Return success result
        return new TakeRequestResult
        {
            RequestId = serviceRequest.Id,
            AgentId = command.AgentId,
            Status = serviceRequest.Status,
            TakenAtUtc = DateTime.UtcNow,
            Message = $"Request '{serviceRequest.RequestNumber}' has been successfully taken."
        };
    }
}
