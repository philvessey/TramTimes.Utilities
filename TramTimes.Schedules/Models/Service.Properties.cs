using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Schedules.Models;

public partial class Service
{
    [UsedImplicitly]
    [JsonPropertyName("departure_time")]
    public string? DepartureTime { get; init; }
    
    [UsedImplicitly]
    [JsonPropertyName("destination_name")]
    public string? DestinationName { get; init; }
    
    [UsedImplicitly]
    [JsonPropertyName("route_name")]
    public string? RouteName { get; init; }
}