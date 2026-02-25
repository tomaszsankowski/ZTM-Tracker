using MediatR;

namespace VueZtmBackend.Application.UserStops.Queries;

public record GetUserStopsQuery(Guid UserId) : IRequest<GetUserStopsResult>;

public record GetUserStopsResult(
    bool Success,
    string? Message,
    IEnumerable<UserStopDto> UserStops
);

public record UserStopDto(
    Guid Id,
    int StopId,
    string? CustomName,
    DateTime CreatedAt
);

