using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VueZtmBackend.Application.Common.Interfaces;
using VueZtmBackend.Application.Common.Models;

namespace VueZtmBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZtmController : ControllerBase
{
    private readonly IZtmApiService _ztmApiService;

    public ZtmController(IZtmApiService ztmApiService)
    {
        _ztmApiService = ztmApiService;
    }

    /// <summary>
    /// Pobierz listę wszystkich przystanków ZTM (cache'owane)
    /// </summary>
    [HttpGet("stops")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<ZtmStopDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStops(CancellationToken cancellationToken)
    {
        var stops = await _ztmApiService.GetAllStopsAsync(cancellationToken);
        return Ok(stops);
    }

    /// <summary>
    /// Wyszukaj przystanki po nazwie
    /// </summary>
    [HttpGet("stops/search")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<ZtmStopDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchStops([FromQuery] string query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new { message = "Query parameter is required" });
        }

        var stops = await _ztmApiService.GetAllStopsAsync(cancellationToken);
        var filteredStops = stops.Where(s =>
            (s.StopName?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (s.StopDesc?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
            s.StopId.ToString().Contains(query)
        ).Take(50);

        return Ok(filteredStops);
    }

    /// <summary>
    /// Pobierz opóźnienia dla konkretnego przystanku
    /// </summary>
    [HttpGet("delays/{stopId:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ZtmDelaysResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDelays(int stopId, CancellationToken cancellationToken)
    {
        var delays = await _ztmApiService.GetDelaysForStopAsync(stopId, cancellationToken);

        if (delays == null)
        {
            return NotFound(new { message = $"Could not fetch delays for stop {stopId}" });
        }

        return Ok(delays);
    }
}

