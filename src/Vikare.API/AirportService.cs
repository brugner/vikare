using AutoMapper;

public class AirportService : IAirportService
{
    private readonly IAirportRepository _repository;
    private readonly IMapper _mapper;

    public AirportService(IAirportRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all the airports.
    /// </summary>
    /// <param name="listParams">Pagination and search parameters.</param>
    /// <returns></returns>
    public AirportListResult GetAll(AirportListParams listParams)
    {
        var airports = _repository.Airports;

        if (!string.IsNullOrEmpty(listParams.Search))
            airports = airports
                .Where(x =>
                    x.Name.Contains(listParams.Search, StringComparison.OrdinalIgnoreCase) ||
                    x.City.Contains(listParams.Search, StringComparison.OrdinalIgnoreCase) ||
                    x.Country.Contains(listParams.Search, StringComparison.OrdinalIgnoreCase))
                .ToList();

        airports = airports
            .Skip((listParams.Page - 1) * listParams.PageSize)
            .Take(listParams.PageSize)
            .ToList();

        var metadata = new AirportListResultMetadata
        {
            Page = listParams.Page,
            PageSize = listParams.PageSize,
            Total = _repository.Airports.Count
        };

        return new AirportListResult
        {
            Metadata = listParams.ExcludeMetadata ? null : metadata,
            Data = airports
        };
    }

    /// <summary>
    /// Get an airport by Id.
    /// </summary>
    /// <param name="id">Airport Id.</param>
    /// <returns></returns>
    public Airport? GetById(int id)
    {
        return _repository.Airports.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Get a list of the closest or farthest airports relative to a set of coordinates.
    /// </summary>
    /// <param name="direction">Closest or farthest.</param>
    /// <param name="lat">Latitude.</param>
    /// <param name="lng">Longitude.</param>
    /// <param name="count">Indicates how many airports to include in the result.</param>
    /// <returns></returns>
    public List<AirportDistance> GetAirportsByDistance(AirportsByDistanceParams airportsByDistanceParams)
    {
        var geoHelper = new GeoHelper();
        var airportsByDistance = _mapper.Map<List<AirportDistance>>(_repository.Airports);

        foreach (var airport in airportsByDistance)
        {
            var distance = geoHelper.GetDistance(airportsByDistanceParams.Latitude, airportsByDistanceParams.Longitude, airport.Latitude, airport.Longitude, airportsByDistanceParams.Unit);
            airport.Distance = distance;
        }

        if (airportsByDistanceParams.Direction == Direction.Closest)
        {
            airportsByDistance = airportsByDistance.OrderBy(x => x.Distance).ToList();
        }
        else
        {
            airportsByDistance = airportsByDistance.OrderByDescending(x => x.Distance).ToList();
        }

        return airportsByDistance
            .Take(airportsByDistanceParams.Count)
            .ToList();
    }
}