namespace RealTimeOpsPortal.Domain.Requests;

public class ServiceRequest
{
    private readonly List<RequestStatusHistory> _statusHistory = [];

    public Guid Id { get; private set; }

    public string RequestNumber { get; private set; } = string.Empty;

    public Guid CustomerId { get; private set; }

    public Guid? AssignedUserId { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public RequestPriority Priority { get; private set; }

    public RequestStatus Status { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }

    public IReadOnlyCollection<RequestStatusHistory> StatusHistory
        => _statusHistory.AsReadOnly();

    private ServiceRequest()
    {
    }

    public ServiceRequest(
        Guid customerId,
        string description,
        RequestPriority priority)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException(
                "Customer ID is required.",
                nameof(customerId));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Description is required.",
                nameof(description));

        Id = Guid.NewGuid();

        CustomerId = customerId;

        Description = description.Trim();

        Priority = priority;

        Status = RequestStatus.Submitted;

        CreatedAtUtc = DateTime.UtcNow;

        RequestNumber = GenerateRequestNumber();

        AddStatusHistory(
            fromStatus: null,
            toStatus: RequestStatus.Submitted,
            changedByUserId: customerId);
    }

    private string GenerateRequestNumber()
    {
        return $"REQ-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(10000, 99999)}";
    }

    private void AddStatusHistory(
        RequestStatus? fromStatus,
        RequestStatus toStatus,
        Guid changedByUserId)
    {
        _statusHistory.Add(
            new RequestStatusHistory(
                Id,
                fromStatus,
                toStatus,
                changedByUserId,
                DateTime.UtcNow));
    }

    public void AssignTo(
    Guid employeeId,
    Guid performedByUserId)
    {
        if (employeeId == Guid.Empty)
            throw new ArgumentException(
                "Employee ID is required.",
                nameof(employeeId));

        if (AssignedUserId.HasValue)
            throw new InvalidOperationException(
                "Request is already assigned.");

        AssignedUserId = employeeId;

        ChangeStatus(
            RequestStatus.Assigned,
            performedByUserId);
    }

    public void ChangeStatus(
    RequestStatus newStatus,
    Guid changedByUserId)
    {
        if (changedByUserId == Guid.Empty)
            throw new ArgumentException(
                "Changed by user ID is required.",
                nameof(changedByUserId));

        if (Status == newStatus)
            return;

        if (!CanTransitionTo(newStatus))
        {
            throw new InvalidOperationException(
                $"Cannot change request status from {Status} to {newStatus}.");
        }

        var previousStatus = Status;

        Status = newStatus;
        UpdatedAtUtc = DateTime.UtcNow;

        AddStatusHistory(
            previousStatus,
            newStatus,
            changedByUserId);
    }

    private bool CanTransitionTo(RequestStatus newStatus)
    {
        return Status switch
        {
            RequestStatus.Submitted =>
                newStatus is
                    RequestStatus.UnderReview or
                    RequestStatus.Assigned or
                    RequestStatus.Cancelled,

            RequestStatus.UnderReview =>
                newStatus is
                    RequestStatus.Assigned or
                    RequestStatus.Rejected or
                    RequestStatus.Cancelled,

            RequestStatus.Assigned =>
                newStatus is
                    RequestStatus.Processing or
                    RequestStatus.Escalated,

            RequestStatus.Processing =>
                newStatus is
                    RequestStatus.WaitingCustomer or
                    RequestStatus.WaitingApproval or
                    RequestStatus.Completed or
                    RequestStatus.Escalated or
                    RequestStatus.Failed,

            RequestStatus.WaitingCustomer =>
                newStatus is
                    RequestStatus.Processing or
                    RequestStatus.Cancelled,

            RequestStatus.WaitingApproval =>
                newStatus is
                    RequestStatus.Processing or
                    RequestStatus.Completed or
                    RequestStatus.Rejected,

            RequestStatus.Escalated =>
                newStatus is
                    RequestStatus.Assigned or
                    RequestStatus.Processing or
                    RequestStatus.Rejected,

            RequestStatus.Completed => false,

            RequestStatus.Rejected => false,

            RequestStatus.Cancelled => false,

            RequestStatus.Failed =>
                newStatus is RequestStatus.Processing,

            _ => false
        };
    }
}
