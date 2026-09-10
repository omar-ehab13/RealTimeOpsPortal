using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeOpsPortal.Application.Authentication;
using RealTimeOpsPortal.Application.Authorization;
using RealTimeOpsPortal.Application.Requests.Create;
using RealTimeOpsPortal.Application.Requests.GetById;
using RealTimeOpsPortal.Application.Requests.GetMy;
using RealTimeOpsPortal.Application.Requests.Take;
using RealTimeOpsPortal.Domain.Users;
using System.IdentityModel.Tokens.Jwt;

namespace RealTimeOpsPortal.Api.Controllers;

[ApiController]
[Route("api/requests")]
[Authorize]
public sealed class RequestsController : ControllerBase
{
    private readonly ICurrentUserContext _currentUser;
    private readonly IAuthorizationService _authorizationService;

    public RequestsController(ICurrentUserContext currentUser, IAuthorizationService authorizationService)
    {
        this._currentUser = currentUser;
        this._authorizationService = authorizationService;
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Customer))]
    public async Task<IActionResult> CreateAsync(
        CreateServiceRequest request,
        [FromServices] CreateServiceRequestUseCase createRequestUseCase,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.UserId.HasValue)
            return Unauthorized();

        var result = await createRequestUseCase.ExecuteAsync(_currentUser.UserId.Value, request, cancellationToken);

        return Created($"api/requests/{result.Id}", result);
    }

    [HttpGet]
    [Authorize(Roles = nameof(UserRole.Customer))]
    public async Task<IActionResult> GetMineAsync(
        [FromServices] GetMyServiceRequestsUseCase getMyRequestsUseCase,
        CancellationToken cancellationToken)
    {
        var result =
            await getMyRequestsUseCase.ExecuteAsync(
                _currentUser.UserId!.Value,
                cancellationToken);

        return Ok(result);
    }

    [HttpGet("{requestId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        Guid requestId,
        [FromServices] GetServiceRequestByIdUseCase getServiceRequestByIdUseCase,
        CancellationToken cancellationToken)
    {
        var request =
            await getServiceRequestByIdUseCase.ExecuteAsync(
                requestId,
                cancellationToken);

        if (request is null)
            return NotFound();

        var authorizationResult =
            await _authorizationService.AuthorizeAsync(
                User,
                request,
                AuthorizationPolicyNames.CanAccessRequest);

        if (!authorizationResult.Succeeded)
        {
            return NotFound();
        }

        return Ok(request);
    }

    [HttpPost("{requestId:guid}/take")]
    [Authorize(Roles = nameof(UserRole.Agent))]
    public async Task<IActionResult> TakeAsync(
        Guid requestId,
        [FromServices] TakeRequestUseCase takeRequestUseCase,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.UserId.HasValue)
            return Unauthorized();

        try
        {
            var command = new TakeRequestCommand
            {
                RequestId = requestId,
                AgentId = _currentUser.UserId.Value
            };

            var result = await takeRequestUseCase.ExecuteAsync(command, cancellationToken);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("already assigned"))
        {
            return Conflict(new { error = ex.Message });
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("modified by another user"))
        {
            // Concurrency conflict: another agent took the request first
            return Conflict(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
