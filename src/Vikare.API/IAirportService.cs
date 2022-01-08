public interface IAirportService
{
    List<AirportDistance> GetAirportsByDistance(AirportsByDistanceParams airportsByDistanceParams);
    AirportListResult GetAll(AirportListParams listParams);
    Airport? GetById(int id);
}