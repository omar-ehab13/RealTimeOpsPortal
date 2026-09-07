namespace RealTimeOpsPortal.Infrastructure.Persistence.Seed;

public static class DevelopmentSeedConstants
{
    public const string DefaultPassword = "P@ssw0rd123!";

    public static class Customers
    {
        public const string Email = "customer@portal.local";
        public const string DisplayName = "Demo Customer";
    }

    public static class Agents
    {
        public const string Email = "agent@portal.local";
        public const string DisplayName = "Demo Agent";
    }

    public static class Operations
    {
        public const string Email = "operations@portal.local";
        public const string DisplayName = "Demo Operations";
    }

    public static class Supervisors
    {
        public const string Email = "supervisor@portal.local";
        public const string DisplayName = "Demo Supervisor";
    }

    public static class Admins
    {
        public const string Email = "admin@portal.local";
        public const string DisplayName = "Demo Admin";
    }
}