using VueZtmBackend.Application.Common.Models;

namespace VueZtmBackend.Application.Common.Interfaces;

public interface IZtmApiService
{
    Task<IEnumerable<ZtmStopDto>> GetAllStopsAsync(CancellationToken cancellationToken = default);
    Task<ZtmDelaysResponseDto?> GetDelaysForStopAsync(int stopId, CancellationToken cancellationToken = default);
}

