using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Schedules.Models;

public class Schedule
{
    [UsedImplicitly]
    [JsonPropertyName("monday")]
    public List<Service>? Monday { get; init; }

    [UsedImplicitly]
    [JsonPropertyName("tuesday")]
    public List<Service>? Tuesday { get; init; }

    [UsedImplicitly]
    [JsonPropertyName("wednesday")]
    public List<Service>? Wednesday { get; init; }

    [UsedImplicitly]
    [JsonPropertyName("thursday")]
    public List<Service>? Thursday { get; init; }

    [UsedImplicitly]
    [JsonPropertyName("friday")]
    public List<Service>? Friday { get; init; }

    [UsedImplicitly]
    [JsonPropertyName("saturday")]
    public List<Service>? Saturday { get; init; }

    [UsedImplicitly]
    [JsonPropertyName("sunday")]
    public List<Service>? Sunday { get; init; }
}