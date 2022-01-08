public record Airport
{
    /// <summary>
    /// Unique OpenFlights identifier for this airport.
    /// </summary>
    public int Id { get; init; } = default!;

    /// <summary>
    /// Airport name.
    /// </summary>
    public string Name { get; init; } = default!;

    /// <summary>
    /// Main city serverd by the airport.
    /// </summary>
    public string City { get; init; } = default!;

    /// <summary>
    /// Country or territory where the airport is located.
    /// </summary>
    public string Country { get; init; } = default!;

    /// <summary>
    /// 3 letter IATA code. Null if unassigned or unknown.
    /// </summary>
    public string IATA { get; init; } = default!;

    /// <summary>
    /// 4 letter ICAO code. Null if unassigned.
    /// </summary>
    public string ICAO { get; init; } = default!;

    /// <summary>
    /// Airport latitude.
    /// </summary>
    public double Latitude { get; init; } = default!;

    /// <summary>
    /// Airport longitude.
    /// </summary>
    public double Longitude { get; init; } = default!;

    /// <summary>
    /// Airport altitude expressed in feet.
    /// </summary>
    public short Altitude { get; init; } = default!;

    /// <summary>
    /// Hours offset from UTC. Fractional hours expressed as decimal.
    /// </summary>
    public string Timezone { get; init; } = default!;

    /// <summary>
    /// Daylight savings time. One of E (Europe), A (US/Canada), S (South America), O (Australia), Z (New Zealand), N (None) or U (Unknown). 
    /// </summary>
    public string DST { get; init; } = default!;

    /// <summary>
    /// Timezone in "tz" (Olson) format, eg. "America/Los_Angeles".
    /// </summary>
    public string TZ { get; init; } = default!;

    /// <summary>
    /// Source of this data. "OurAirports" for data sourced from OurAirports, "Legacy" for old data not matched to OurAirports (mostly DAFIF), 
    /// "User" for unverified user contributions. In airports.csv, only source=OurAirports is included.
    /// </summary>
    public string Source { get; init; } = default!;
}