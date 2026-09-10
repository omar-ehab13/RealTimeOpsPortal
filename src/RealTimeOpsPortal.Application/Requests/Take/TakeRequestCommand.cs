namespace RealTimeOpsPortal.Application.Requests.Take;

/// <summary>
/// Command to take (assign to self) a service request by an agent/employee.
/// </summary>
public class TakeRequestCommand
{
    /// <summary>
    /// The unique identifier of the service request to take.
    /// </summary>
    public required Guid RequestId { get; init; }

    /// <summary>
    /// The unique identifier of the agent/employee taking the request.
    /// </summary>
    public required Guid AgentId { get; init; }
}