using MediatR;
using VueZtmBackend.Application.Common.Models;

namespace VueZtmBackend.Application.UserStops.Queries;

public record GetUserStopsWithDelaysQuery(Guid UserId) : IRequest<GetUserStopsWithDelaysResult>;

public record GetUserStopsWithDelaysResult(
    bool Success,
    string? Message,
    IEnumerable<UserStopWithDelaysDto> Stops
);

public record UserStopWithDelaysDto(
    Guid UserStopId,
    int StopId,
    string? CustomName,
    string? StopName,
    string? LastUpdate,
    IEnumerable<ZtmDelayDto> Delays
);

