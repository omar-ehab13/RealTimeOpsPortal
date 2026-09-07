namespace RealTimeOpsPortal.Application.Settings;

/// <summary>
/// JWT authentication settings bound from configuration.
/// </summary>
public class JwtSettings
{
    public const string Section = "Jwt";

    /// <summary>
    /// The secret key used to sign JWT tokens.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// The issuer of the JWT tokens.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// The intended audience of the JWT tokens.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Token expiration time in minutes.
    /// </summary>
    public int ExpirationMinutes { get; set; } = 60;
}
