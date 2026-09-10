using RealTimeOpsPortal.Domain.Users;

namespace RealTimeOpsPortal.Application.Authorization;

public static class RequestAccessPolicy
{
    public static bool CanView(Guid userId, UserRole role, Guid customerId, Guid? assignedUserId)
    {
        return role switch
        {
            UserRole.Customer =>
                customerId == userId,

            UserRole.Agent =>
                assignedUserId is null ||
                assignedUserId == userId,

            UserRole.Operations =>
                true,

            UserRole.Supervisor =>
                true,

            UserRole.Admin =>
                true,

            _ => false
        };
    }
}