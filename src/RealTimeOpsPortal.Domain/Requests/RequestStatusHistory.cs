namespace RealTimeOpsPortal.Domain.Requests;

public class RequestStatusHistory
{
    public long Id { get; private set; }

    public Guid ServiceRequestId { get; private set; }

    public RequestStatus? FromStatus { get; private set; }

    public RequestStatus ToStatus { get; private set; }

    public Guid ChangedByUserId { get; private set; }

    public DateTime ChangedAtUtc { get; private set; }

    private RequestStatusHistory()
    {
    }

    internal RequestStatusHistory(
        Guid serviceRequestId,
        RequestStatus? fromStatus,
        RequestStatus toStatus,
        Guid changedByUserId,
        DateTime changedAtUtc)
    {
        ServiceRequestId = serviceRequestId;
        FromStatus = fromStatus;
        ToStatus = toStatus;
        ChangedByUserId = changedByUserId;
        ChangedAtUtc = changedAtUtc;
    }
}