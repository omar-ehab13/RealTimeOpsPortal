using RealTimeOpsPortal.Domain.Requests;

namespace RealTimeOpsPortal.UnitTests.Requests;

public class ServiceRequestTests
{
    [Fact]
    public void Constructor_Should_Create_Request_In_Submitted_Status()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var request = new ServiceRequest(
            customerId,
            "Internet connection is not working.",
            RequestPriority.High);

        // Assert
        Assert.NotEqual(Guid.Empty, request.Id);
        Assert.Equal(customerId, request.CustomerId);
        Assert.Equal(RequestPriority.High, request.Priority);
        Assert.Equal(RequestStatus.Submitted, request.Status);
        Assert.Null(request.AssignedUserId);

        Assert.Single(request.StatusHistory);
    }

    [Fact]
    public void Constructor_Should_Add_Initial_Status_History() 
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var request = new ServiceRequest(
            customerId,
            "Payment problem.",
            RequestPriority.Normal);

        // Assert
        var history = Assert.Single(request.StatusHistory);

        Assert.Null(history.FromStatus);
        Assert.Equal(RequestStatus.Submitted, history.ToStatus);
        Assert.Equal(customerId, history.ChangedByUserId);
    }

    [Fact]
    public void ChangeStatus_Should_Change_Status_When_Transition_Is_Valid()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();

        var request = new ServiceRequest(
            customerId,
            "Customer needs support.",
            RequestPriority.Normal);

        // Act
        request.ChangeStatus(
            RequestStatus.UnderReview,
            employeeId);

        // Assert
        Assert.Equal(
            RequestStatus.UnderReview,
            request.Status);

        Assert.Equal(2, request.StatusHistory.Count);
    }

    [Fact]
    public void ChangeStatus_Should_Add_Status_History()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();

        var request = new ServiceRequest(
            customerId,
            "Request description.",
            RequestPriority.High);

        // Act
        request.ChangeStatus(
            RequestStatus.UnderReview,
            employeeId);

        // Assert
        var history = request.StatusHistory.Last();

        Assert.Equal(
            RequestStatus.Submitted,
            history.FromStatus);

        Assert.Equal(
            RequestStatus.UnderReview,
            history.ToStatus);

        Assert.Equal(
            employeeId,
            history.ChangedByUserId);
    }

    [Fact]
    public void ChangeStatus_Should_Throw_When_Transition_Is_Invalid()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();

        var request = new ServiceRequest(
            customerId,
            "Request description.",
            RequestPriority.Normal);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(
            () => request.ChangeStatus(
                RequestStatus.Completed,
                employeeId));

        // Assert
        Assert.Equal(
            "Cannot change request status from Submitted to Completed.",
            exception.Message);
    }

    [Fact]
    public void AssignTo_Should_Assign_Employee_And_Change_Status()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var supervisorId = Guid.NewGuid();

        var request = new ServiceRequest(
            customerId,
            "Request description.",
            RequestPriority.High);

        // Act
        request.AssignTo(
            employeeId,
            supervisorId);

        // Assert
        Assert.Equal(
            employeeId,
            request.AssignedUserId);

        Assert.Equal(
            RequestStatus.Assigned,
            request.Status);

        Assert.Equal(
            2,
            request.StatusHistory.Count);
    }

    [Fact]
    public void AssignTo_Should_Throw_When_Request_Is_Already_Assigned()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var employee1Id = Guid.NewGuid();
        var employee2Id = Guid.NewGuid();

        var supervisorId = Guid.NewGuid();

        var request = new ServiceRequest(
            customerId,
            "Request description.",
            RequestPriority.High);

        request.AssignTo(
            employee1Id,
            supervisorId);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(
            () => request.AssignTo(
                employee2Id,
                supervisorId));

        // Assert
        Assert.Equal(
            "Request is already assigned.",
            exception.Message);

        Assert.Equal(
            employee1Id,
            request.AssignedUserId);
    }

    [Fact]
    public void Request_Should_Complete_Through_Valid_Lifecycle()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();

        var request = new ServiceRequest(
            customerId,
            "Request description.",
            RequestPriority.Normal);

        // Act
        request.ChangeStatus(
            RequestStatus.UnderReview,
            employeeId);

        request.AssignTo(
            employeeId,
            employeeId);

        request.ChangeStatus(
            RequestStatus.Processing,
            employeeId);

        request.ChangeStatus(
            RequestStatus.Completed,
            employeeId);

        // Assert
        Assert.Equal(
            RequestStatus.Completed,
            request.Status);

        Assert.Equal(
            5,
            request.StatusHistory.Count);
    }

    [Fact]
    public void ChangeStatus_Should_Throw_When_Request_Is_Already_Completed()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();

        var request = new ServiceRequest(
            customerId,
            "Request description.",
            RequestPriority.Normal);

        request.AssignTo(
            employeeId,
            employeeId);

        request.ChangeStatus(
            RequestStatus.Processing,
            employeeId);

        request.ChangeStatus(
            RequestStatus.Completed,
            employeeId);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(
            () => request.ChangeStatus(
                RequestStatus.Processing,
                employeeId));

        // Assert
        Assert.Contains(
            "Cannot change request status",
            exception.Message);

        Assert.Equal(
            RequestStatus.Completed,
            request.Status);
    }
}