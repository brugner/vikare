public record AirportDistance
{
    public int Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string City { get; init; } = default!;
    public string Country { get; init; } = default!;
    public double Latitude { get; init; } = default!;
    public double Longitude { get; init; } = default!;
    public double Distance { get; internal set; } = default!;
}