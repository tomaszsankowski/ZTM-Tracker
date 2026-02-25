using MediatR;

namespace VueZtmBackend.Application.UserStops.Commands;

public record CreateUserStopCommand(Guid UserId, int StopId, string? CustomName) : IRequest<CreateUserStopResult>;

public record CreateUserStopResult(bool Success, string? Message, Guid? UserStopId);
