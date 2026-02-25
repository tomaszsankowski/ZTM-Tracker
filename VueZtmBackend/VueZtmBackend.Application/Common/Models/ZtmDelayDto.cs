using System.Text.Json.Serialization;

namespace VueZtmBackend.Application.Common.Models;

public class ZtmDelaysResponseDto
{
    [JsonPropertyName("lastUpdate")]
    public string? LastUpdate { get; set; }

    [JsonPropertyName("departures")]
    public List<ZtmDelayDto> Delay { get; set; } = new();
}

public class ZtmDelayDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("delayInSeconds")]
    public int? DelayInSeconds { get; set; }

    [JsonPropertyName("estimatedTime")]
    public string? EstimatedTime { get; set; }

    [JsonPropertyName("headsign")]
    public string? Headsign { get; set; }

    [JsonPropertyName("routeId")]
    public int? RouteId { get; set; }

    [JsonPropertyName("tripId")]
    public int? TripId { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("theoreticalTime")]
    public string? TheoreticalTime { get; set; }

    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; set; }

    [JsonPropertyName("trip")]
    public long? Trip { get; set; }

    [JsonPropertyName("vehicleCode")]
    public int? VehicleCode { get; set; }

    [JsonPropertyName("vehicleId")]
    public int? VehicleId { get; set; }
}

