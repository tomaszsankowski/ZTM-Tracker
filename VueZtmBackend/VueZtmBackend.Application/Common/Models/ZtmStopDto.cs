using System.Text.Json.Serialization;

namespace VueZtmBackend.Application.Common.Models;

public class ZtmStopDto
{
    [JsonPropertyName("stopId")]
    public int StopId { get; set; }

    [JsonPropertyName("stopCode")]
    public string? StopCode { get; set; }

    [JsonPropertyName("stopName")]
    public string? StopName { get; set; }

    [JsonPropertyName("stopShortName")]
    public string? StopShortName { get; set; }

    [JsonPropertyName("stopDesc")]
    public string? StopDesc { get; set; }

    [JsonPropertyName("subName")]
    public string? SubName { get; set; }

    [JsonPropertyName("date")]
    public string? Date { get; set; }

    [JsonPropertyName("zoneId")]
    public int? ZoneId { get; set; }

    [JsonPropertyName("zoneName")]
    public string? ZoneName { get; set; }

    [JsonPropertyName("virtual")]
    public int? Virtual { get; set; }

    [JsonPropertyName("nonpassenger")]
    public int? Nonpassenger { get; set; }

    [JsonPropertyName("depot")]
    public int? Depot { get; set; }

    [JsonPropertyName("ticketZoneBorder")]
    public int? TicketZoneBorder { get; set; }

    [JsonPropertyName("onDemand")]
    public int? OnDemand { get; set; }

    [JsonPropertyName("activationDate")]
    public string? ActivationDate { get; set; }

    [JsonPropertyName("stopLat")]
    public double? StopLat { get; set; }

    [JsonPropertyName("stopLon")]
    public double? StopLon { get; set; }
}

