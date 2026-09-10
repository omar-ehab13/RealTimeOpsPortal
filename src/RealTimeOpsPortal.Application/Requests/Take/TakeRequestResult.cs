using RealTimeOpsPortal.Domain.Requests;

namespace RealTimeOpsPortal.Application.Requests.Take;

/// <summary>
/// Result DTO for a successful take request operation.
/// </summary>
public class TakeRequestResult
{
    /// <summary>
    /// The unique identifier of the request that was taken.
    /// </summary>
    public required Guid RequestId { get; init; }

    /// <summary>
    /// The unique identifier of the agent who took the request.
    /// </summary>
    public required Guid AgentId { get; init; }

    /// <summary>
    /// The current status of the request after being taken.
    /// </summary>
    public required RequestStatus Status { get; init; }

    /// <summary>
    /// Timestamp when the request was taken (UTC).
    /// </summary>
    public required DateTime TakenAtUtc { get; init; }

    /// <summary>
    /// Human-readable message about the operation result.
    /// </summary>
    public string Message { get; init; } = "Request successfully taken.";
}
