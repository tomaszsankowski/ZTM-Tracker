using MediatR;

namespace VueZtmBackend.Application.UserStops.Commands;

public record UpdateUserStopCommand(Guid UserId, Guid UserStopId, int? StopId, string? CustomName) : IRequest<UpdateUserStopResult>;

public record UpdateUserStopResult(bool Success, string? Message);

