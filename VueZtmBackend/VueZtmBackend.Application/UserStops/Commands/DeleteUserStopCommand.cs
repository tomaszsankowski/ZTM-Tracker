using MediatR;

namespace VueZtmBackend.Application.UserStops.Commands;

public record DeleteUserStopCommand(Guid UserId, Guid UserStopId) : IRequest<DeleteUserStopResult>;

public record DeleteUserStopResult(bool Success, string? Message);

