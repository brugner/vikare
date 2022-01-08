public record AirportsByDistanceParams
{
    private char[] _validUnits = new char[] { 'k', 'm', 'n' };

    public Direction Direction { get; init; } = default!;
    public double Latitude { get; init; } = default!;
    public double Longitude { get; init; } = default!;
    public int Count { get; init; } = default!;
    public char Unit { get; init; } = default!;

    public AirportsByDistanceParams(Direction direction, double lat, double lng, int? count, char? unit)
    {
        if (!count.HasValue || count < 1)
            count = 10;

        if (count > 50)
            count = 50;

        if (unit == null || !_validUnits.Contains(unit.Value))
            unit = 'k';

        Direction = direction;
        Latitude = lat;
        Longitude = lng;
        Count = count.Value;
        Unit = unit.Value;
    }
}