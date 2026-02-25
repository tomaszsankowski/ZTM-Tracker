using MediatR;
using VueZtmBackend.Application.Common.Interfaces;
using VueZtmBackend.Application.UserStops.Commands;
using VueZtmBackend.Application.UserStops.Queries;
using VueZtmBackend.Domain.Entities;
using VueZtmBackend.Domain.Exceptions;
using VueZtmBackend.Domain.Interfaces;
using VueZtmBackend.Domain.ValueObjects;

namespace VueZtmBackend.Application.UserStops.Handlers;

public class CreateUserStopHandler : IRequestHandler<CreateUserStopCommand, CreateUserStopResult>
{
    private readonly IUserStopRepository _userStopRepository;
    private readonly IZtmApiService _ztmApiService;

    public CreateUserStopHandler(IUserStopRepository userStopRepository, IZtmApiService ztmApiService)
    {
        _userStopRepository = userStopRepository;
        _ztmApiService = ztmApiService;
    }

    public async Task<CreateUserStopResult> Handle(CreateUserStopCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var stops = await _ztmApiService.GetAllStopsAsync(cancellationToken);
            if (stops.All(s => s.StopId != request.StopId))
            {
                return new CreateUserStopResult(false, "Podany przystanek nie istnieje.", null);
            }

            if (await _userStopRepository.ExistsAsync(request.UserId, request.StopId, cancellationToken))
            {
                return new CreateUserStopResult(false, "Ten przystanek już został dodany.", null);
            }

            var stopId = new StopId(request.StopId);
            var userStop = new UserStop(request.UserId, stopId, request.CustomName);

            await _userStopRepository.AddAsync(userStop, cancellationToken);

            return new CreateUserStopResult(true, "Przystanek został dodany.", userStop.Id);
        }
        catch (DomainException ex)
        {
            return new CreateUserStopResult(false, ex.Message, null);
        }
    }
}

public class UpdateUserStopHandler : IRequestHandler<UpdateUserStopCommand, UpdateUserStopResult>
{
    private readonly IUserStopRepository _userStopRepository;
    private readonly IZtmApiService _ztmApiService;

    public UpdateUserStopHandler(IUserStopRepository userStopRepository, IZtmApiService ztmApiService)
    {
        _userStopRepository = userStopRepository;
        _ztmApiService = ztmApiService;
    }

    public async Task<UpdateUserStopResult> Handle(UpdateUserStopCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userStop = await _userStopRepository.GetByIdAsync(request.UserStopId, cancellationToken);

            if (userStop is null || userStop.UserId != request.UserId)
            {
                return new UpdateUserStopResult(false, "Przystanek nie został znaleziony.");
            }

            if (request.StopId.HasValue)
            {
                var stops = await _ztmApiService.GetAllStopsAsync(cancellationToken);
                if (stops.All(s => s.StopId != request.StopId.Value))
                {
                    return new UpdateUserStopResult(false, "Podany przystanek nie istnieje.");
                }

                var newStopId = new StopId(request.StopId.Value);
                userStop.UpdateStopId(newStopId);
            }

            if (request.CustomName is not null)
            {
                userStop.UpdateCustomName(request.CustomName);
            }

            await _userStopRepository.UpdateAsync(userStop, cancellationToken);

            return new UpdateUserStopResult(true, "Przystanek został zaktualizowany.");
        }
        catch (DomainException ex)
        {
            return new UpdateUserStopResult(false, ex.Message);
        }
    }
}

public class DeleteUserStopHandler : IRequestHandler<DeleteUserStopCommand, DeleteUserStopResult>
{
    private readonly IUserStopRepository _userStopRepository;

    public DeleteUserStopHandler(IUserStopRepository userStopRepository)
    {
        _userStopRepository = userStopRepository;
    }

    public async Task<DeleteUserStopResult> Handle(DeleteUserStopCommand request, CancellationToken cancellationToken)
    {
        var userStop = await _userStopRepository.GetByIdAsync(request.UserStopId, cancellationToken);

        if (userStop is null || userStop.UserId != request.UserId)
        {
            return new DeleteUserStopResult(false, "Przystanek nie został znaleziony.");
        }

        await _userStopRepository.DeleteAsync(userStop, cancellationToken);

        return new DeleteUserStopResult(true, "Przystanek został usunięty.");
    }
}

public class GetUserStopsHandler : IRequestHandler<GetUserStopsQuery, GetUserStopsResult>
{
    private readonly IUserStopRepository _userStopRepository;

    public GetUserStopsHandler(IUserStopRepository userStopRepository)
    {
        _userStopRepository = userStopRepository;
    }

    public async Task<GetUserStopsResult> Handle(GetUserStopsQuery request, CancellationToken cancellationToken)
    {
        var userStops = await _userStopRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        var dtos = userStops.Select(us => new UserStopDto(
            us.Id,
            us.StopId.Value,
            us.CustomName,
            us.CreatedAt
        ));

        return new GetUserStopsResult(true, null, dtos);
    }
}

public class GetUserStopsWithDelaysHandler : IRequestHandler<GetUserStopsWithDelaysQuery, GetUserStopsWithDelaysResult>
{
    private readonly IUserStopRepository _userStopRepository;
    private readonly IZtmApiService _ztmApiService;

    public GetUserStopsWithDelaysHandler(
        IUserStopRepository userStopRepository,
        IZtmApiService ztmApiService)
    {
        _userStopRepository = userStopRepository;
        _ztmApiService = ztmApiService;
    }

    public async Task<GetUserStopsWithDelaysResult> Handle(GetUserStopsWithDelaysQuery request, CancellationToken cancellationToken)
    {
        var userStops = await _userStopRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        var stops = await _ztmApiService.GetAllStopsAsync(cancellationToken);
        var stopsDict = stops.ToDictionary(s => s.StopId, s => s.StopName);

        var result = new List<UserStopWithDelaysDto>();

        foreach (var userStop in userStops)
        {
            var delays = await _ztmApiService.GetDelaysForStopAsync(userStop.StopId.Value, cancellationToken);
            stopsDict.TryGetValue(userStop.StopId.Value, out var stopName);

            result.Add(new UserStopWithDelaysDto(
                userStop.Id,
                userStop.StopId.Value,
                userStop.CustomName,
                stopName,
                delays?.LastUpdate,
                delays?.Delay ?? Enumerable.Empty<Common.Models.ZtmDelayDto>()
            ));
        }

        return new GetUserStopsWithDelaysResult(true, null, result);
    }
}

