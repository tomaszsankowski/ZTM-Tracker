using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VueZtmBackend.Application.UserStops.Commands;
using VueZtmBackend.Application.UserStops.Queries;

namespace VueZtmBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserStopsController : ControllerBase

{
    private readonly IMediator _mediator;
    public UserStopsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user token");
        }

        return userId;
    }

    /// <summary>
    /// Pobierz zapisane przystanki użytkownika
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(GetUserStopsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserStops(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var query = new GetUserStopsQuery(userId);
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result.UserStops);
    }

    /// <summary>
    /// Pobierz przystanki użytkownika z danymi o opóźnieniach ZTM
    /// </summary>
    [HttpGet("with-delays")]
    [ProducesResponseType(typeof(GetUserStopsWithDelaysResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserStopsWithDelays(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var query = new GetUserStopsWithDelaysQuery(userId);
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result.Stops);
    }

    /// <summary>
    /// Dodaj nowy przystanek
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateUserStopResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserStop([FromBody] CreateUserStopRequest request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var command = new CreateUserStopCommand(userId, request.StopId, request.CustomName);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }

        return CreatedAtAction(nameof(GetUserStops), new { id = result.UserStopId }, new { id = result.UserStopId, message = result.Message });
    }

    /// <summary>
    /// Zaktualizuj przystanek
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserStop(Guid id, [FromBody] UpdateUserStopRequest request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var command = new UpdateUserStopCommand(userId, id, request.StopId, request.CustomName);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return NotFound(new { message = result.Message });
        }

        return Ok(new { message = result.Message });
    }

    /// <summary>
    /// Usuń przystanek
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserStop(Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var command = new DeleteUserStopCommand(userId, id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return NotFound(new { message = result.Message });
        }

        return Ok(new { message = result.Message });
    }
}

public record CreateUserStopRequest(int StopId, string? CustomName);
public record UpdateUserStopRequest(int? StopId, string? CustomName);

