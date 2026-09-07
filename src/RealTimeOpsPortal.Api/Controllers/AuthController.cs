using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeOpsPortal.Application.Authentication;
using Auth = RealTimeOpsPortal.Application.Authentication;

namespace RealTimeOpsPortal.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;

    public AuthController(LoginUseCase loginUseCase)
    {
        _loginUseCase = loginUseCase;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(Auth.LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _loginUseCase.ExecuteAsync(request, cancellationToken);

        if (result is null)
        {
            return Unauthorized(
                new
                {
                    message = "Invalid email or password."
                });
        }

        return Ok(result);
    }
}