using System.Text.Json.Serialization;

public record AirportListResult
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AirportListResultMetadata? Metadata { get; init; } = default;
    public List<Airport> Data { get; init; } = default!;
}