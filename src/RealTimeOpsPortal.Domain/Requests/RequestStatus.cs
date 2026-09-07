namespace RealTimeOpsPortal.Domain.Requests;

public enum RequestStatus
{
    Submitted = 1,

    UnderReview = 2,

    Assigned = 3,

    Processing = 4,

    WaitingCustomer = 5,

    WaitingApproval = 6,

    Completed = 7,

    Rejected = 8,

    Cancelled = 9,

    Escalated = 10,

    Failed = 11
}