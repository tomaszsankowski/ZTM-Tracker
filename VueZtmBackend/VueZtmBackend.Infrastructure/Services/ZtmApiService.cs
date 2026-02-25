﻿using System.Text.Json;    
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using VueZtmBackend.Application.Common.Interfaces;
using VueZtmBackend.Application.Common.Models;

namespace VueZtmBackend.Infrastructure.Services;

public class ZtmApiService : IZtmApiService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ZtmApiService> _logger;

    private const string StopsUrl = "https://ckan.multimediagdansk.pl/dataset/c24aa637-3619-4dc2-a171-a23eec8f2172/resource/4c4025f0-01bf-41f7-a39f-d156d201b82b/download/stops.json";
    private const string DelaysUrlTemplate = "http://ckan2.multimediagdansk.pl/departures?stopId={0}";
    private const string StopsCacheKey = "ZtmStops";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

    public ZtmApiService(HttpClient httpClient, IMemoryCache cache, ILogger<ZtmApiService> logger)
    {
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
    }

    public async Task<IEnumerable<ZtmStopDto>> GetAllStopsAsync(CancellationToken cancellationToken = default)
{
    // 1. Sprawdź cache (bez zmian)
    if (_cache.TryGetValue(StopsCacheKey, out IEnumerable<ZtmStopDto>? cachedStops) && cachedStops != null)
    {
        _logger.LogInformation("Returning cached stops");
        return cachedStops;
    }

    try
    {
        _logger.LogInformation("Fetching stops from ZTM API");

        using var response = await _httpClient.GetAsync(StopsUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

        var todayKey = DateTime.Now.ToString("yyyy-MM-dd");
        JsonElement stopsElement = default;
        bool found = false;

        if (doc.RootElement.TryGetProperty(todayKey, out var dateEntry))
        {
             stopsElement = dateEntry.GetProperty("stops");
             found = true;
             _logger.LogInformation("Found stops for today: {Date}", todayKey);
        }
        else
        {
            var firstProp = doc.RootElement.EnumerateObject().FirstOrDefault();
            if (firstProp.Value.ValueKind != JsonValueKind.Undefined)
            {
                stopsElement = firstProp.Value.GetProperty("stops");
                found = true;
                _logger.LogWarning("Stops for today ({Today}) not found. Using fallback date: {Date}", todayKey, firstProp.Name);
            }
        }

        List<ZtmStopDto> stops = new();

        if (found)
        {
            stops = stopsElement.Deserialize<List<ZtmStopDto>>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<ZtmStopDto>();
        }

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(CacheDuration)
            .SetPriority(CacheItemPriority.High);

        _cache.Set(StopsCacheKey, stops, cacheOptions);
        
        return stops;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error fetching stops from ZTM API");
        return Enumerable.Empty<ZtmStopDto>();
    }
}

    public async Task<ZtmDelaysResponseDto?> GetDelaysForStopAsync(int stopId, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = string.Format(DelaysUrlTemplate, stopId);
            _logger.LogDebug("Fetching delays for stop {StopId}", stopId);

            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var delays = JsonSerializer.Deserialize<ZtmDelaysResponseDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return delays;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching delays for stop {StopId}", stopId);
            return null;
        }
    }
}

/// <summary>
/// Struktura odpowiedzi API ZTM dla przystanków.
/// Format: { "2025-12-08": { "lastUpdate": "...", "stops": [...] }, "2025-12-09": { ... } }
/// </summary>
internal class ZtmDateEntry
{
    [System.Text.Json.Serialization.JsonPropertyName("lastUpdate")]
    public string? LastUpdate { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("stops")]
    public List<ZtmStopDto>? Stops { get; set; }
}

