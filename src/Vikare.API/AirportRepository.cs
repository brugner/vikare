using System.Text.RegularExpressions;

public class AirportRepository : IAirportRepository
{
    private List<Airport>? _airports;

    public List<Airport> Airports
    {
        get
        {
            if (_airports == null)
            {
                _airports = LoadAirportsDataFromCsv();
            }

            return _airports;
        }
    }

    public Airport? GetById(int id)
    {
        return Airports.FirstOrDefault(x => x.Id == id);
    }

    private List<Airport> LoadAirportsDataFromCsv()
    {
        var airports = new List<Airport>();
        var csvParserRegex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        using var reader = new StreamReader("data.csv");

        reader.ReadLine();

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? "";

            var values = csvParserRegex.Split(line);

            if (values == null)
                continue;

            airports.Add(new Airport
            {
                Id = Convert.ToUInt16(values[0]),
                Name = values[1],
                City = values[2],
                Country = values[3],
                IATA = values[4],
                ICAO = values[5],
                Latitude = Convert.ToDouble(values[6]),
                Longitude = Convert.ToDouble(values[7]),
                Altitude = Convert.ToInt16(values[8]),
                Timezone = values[9],
                DST = values[10],
                TZ = values[11],
                Source = values[13]
            });
        }

        return airports;
    }
}