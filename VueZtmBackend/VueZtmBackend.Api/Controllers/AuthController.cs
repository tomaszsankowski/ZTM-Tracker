using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VueZtmBackend.Application.Auth.Commands;

namespace VueZtmBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Rejestracja nowego użytkownika
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RegisterResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(request.Login, request.Password);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }

        return Ok(new { message = result.Message, userId = result.UserId });
    }

    /// <summary>
    /// Logowanie użytkownika
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Login, request.Password);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return Unauthorized(new { message = result.Message });
        }

        return Ok(new LoginResponse(
            result.AccessToken!,
            result.RefreshToken!,
            result.ExpiresAt!.Value
        ));
    }

    /// <summary>
    /// Odświeżenie tokenu dostępu
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(request.RefreshToken);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return Unauthorized(new { message = result.Message });
        }

        return Ok(new LoginResponse(
            result.AccessToken!,
            result.RefreshToken!,
            result.ExpiresAt!.Value
        ));
    }
}

public record RegisterRequest(string Login, string Password);
public record LoginRequest(string Login, string Password);
public record RefreshTokenRequest(string RefreshToken);
public record LoginResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt);
