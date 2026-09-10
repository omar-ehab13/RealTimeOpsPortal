using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RealTimeOpsPortal.Application.Authentication;
using RealTimeOpsPortal.Domain.Users;

namespace RealTimeOpsPortal.Infrastructure.Authentication;

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(ApplicationUser user)
    {
        var issuer =
            _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException(
                "JWT issuer is not configured.");

        var audience =
            _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException(
                "JWT audience is not configured.");

        var key =
            _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "JWT key is not configured.");

        var expirationMinutes =
            int.Parse(
                _configuration["Jwt:ExpirationMinutes"]
                ?? "60");

        var claims = new[]
        {
            new Claim(
                ClaimNames.Subject,
                user.Id.ToString()),

            new Claim(
                ClaimNames.Email,
                user.Email),

            new Claim(
                ClaimNames.Role,
                user.Role.ToString()),

            new Claim(
                ClaimNames.DisplayName,
                user.DisplayName)
        };

        var signingKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

        var credentials =
            new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow
                    .AddMinutes(expirationMinutes),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}