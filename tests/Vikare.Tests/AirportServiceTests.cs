using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using Moq;
using Xunit;

namespace Vikare.Tests;

public class AirportServiceTests
{
    private readonly AirportService _service;
    private readonly Mock<IAirportRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public AirportServiceTests()
    {
        _mapperMock = new Mock<IMapper>();

        var airports = GetAirports();
        _repositoryMock = new Mock<IAirportRepository>();
        _repositoryMock.Setup(x => x.Airports)
            .Returns(airports)
            .Verifiable();

        _service = new AirportService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void GetById_Exists_ReturnsAirport()
    {
        // Act
        var result = _service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result?.Id);

        _repositoryMock.Verify(x => x.Airports, Times.Once());
    }

    [Fact]
    public void GetById_NotExists_ReturnsNull()
    {
        // Act
        var result = _service.GetById(100);

        // Assert
        Assert.Null(result);

        _repositoryMock.Verify(x => x.Airports, Times.Once());
    }


    [Fact]
    public void GetAll_1Page10PageSizeNoSearch_Returns10Airports()
    {
        // Arrange
        var listParams = new AirportListParams(1, 10, string.Empty, false);

        // Act
        var result = _service.GetAll(listParams);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Data.Count);
        Assert.Equal(1, result.Metadata?.Page);
        Assert.Equal(10, result.Metadata?.PageSize);
        Assert.Equal(20, result.Metadata?.Total);

        _repositoryMock.Verify(x => x.Airports, Times.Exactly(2));
    }

    [Fact]
    public void GetAll_1Page10PageSizeSearchReykjavik_Returns1Airport()
    {
        // Arrange
        var listParams = new AirportListParams(1, 10, "Reykjavik", false);

        // Act
        var result = _service.GetAll(listParams);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Data.Count);
        Assert.Equal(1, result.Metadata?.Page);
        Assert.Equal(10, result.Metadata?.PageSize);
        Assert.Equal(20, result.Metadata?.Total);

        _repositoryMock.Verify(x => x.Airports, Times.Exactly(2));
    }

    [Fact]
    public void GetAll_ExcludeMetadata_ReturnsNullMetadata()
    {
        // Arrange
        var listParams = new AirportListParams(1, 10, string.Empty, true);

        // Act
        var result = _service.GetAll(listParams);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Metadata);

        _repositoryMock.Verify(x => x.Airports, Times.Exactly(2));
    }

    [Fact]
    public void GetAirportsByDistance_3Closest_Returns3ClosestAirports()
    {
        // Arrange
        _mapperMock.Setup(x => x.Map<List<AirportDistance>>(It.IsAny<List<Airport>>())).Returns(GetAirportsByDistanceList());
        var airportsByDistanceParams = new AirportsByDistanceParams(Direction.Closest, 50, -50, 3, 'k');

        // Act
        var result = _service.GetAirportsByDistance(airportsByDistanceParams);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.True(result[0].Distance > 0);
        Assert.True(result[1].Distance > 0);
        Assert.True(result[0].Distance <= result[1].Distance);

        _repositoryMock.Verify(x => x.Airports, Times.Once());
    }

    [Fact]
    public void GetAirportsByDistance_1Closest_ReturnsClosestAirport()
    {
        // Arrange
        _mapperMock.Setup(x => x.Map<List<AirportDistance>>(It.IsAny<List<Airport>>())).Returns(GetAirportsByDistanceList());
        var airportsByDistanceParams = new AirportsByDistanceParams(Direction.Closest, 50, -50, 1, 'n');

        // Act
        var result = _service.GetAirportsByDistance(airportsByDistanceParams);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count());
        Assert.True(result[0].Distance > 0);

        _repositoryMock.Verify(x => x.Airports, Times.Once());
    }

    [Fact]
    public void GetAirportsByDistance_5Farthest_Returns5FarthestAirports()
    {
        // Arrange
        _mapperMock.Setup(x => x.Map<List<AirportDistance>>(It.IsAny<List<Airport>>())).Returns(GetAirportsByDistanceList());
        var airportsByDistanceParams = new AirportsByDistanceParams(Direction.Farthest, 50, -50, 5, 'm');

        // Act
        var result = _service.GetAirportsByDistance(airportsByDistanceParams);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count());
        Assert.True(result[0].Distance > 0);
        Assert.True(result[1].Distance > 0);
        Assert.True(result[0].Distance >= result[1].Distance);

        _repositoryMock.Verify(x => x.Airports, Times.Once());
    }

    private List<Airport> GetAirports()
    {
        var rawData = new string[]
        {
            "1,Goroka Airport,Goroka,Papua New Guinea,GKA,AYGA,-6.081689835,145.3919983,5282,10,U,Pacific/Port_Moresby,airport,OurAirports",
            "2,Madang Airport,Madang,Papua New Guinea,MAG,AYMD,-5.207079887,145.7890015,20,10,U,Pacific/Port_Moresby,airport,OurAirports",
            "3,Mount Hagen Kagamuga Airport,Mount Hagen,Papua New Guinea,HGU,AYMH,-5.826789856,144.2960052,5388,10,U,Pacific/Port_Moresby,airport,OurAirports",
            "4,Nadzab Airport,Nadzab,Papua New Guinea,LAE,AYNZ,-6.569803,146.725977,239,10,U,Pacific/Port_Moresby,airport,OurAirports",
            "5,Port Moresby Jacksons International Airport,Port Moresby,Papua New Guinea,POM,AYPY,-9.443380356,147.2200012,146,10,U,Pacific/Port_Moresby,airport,OurAirports",
            "6,Wewak International Airport,Wewak,Papua New Guinea,WWK,AYWK,-3.583830118,143.6690063,19,10,U,Pacific/Port_Moresby,airport,OurAirports",
            "7,Narsarsuaq Airport,Narssarssuaq,Greenland,UAK,BGBW,61.16049957,-45.42599869,112,-3,E,America/Godthab,airport,OurAirports",
            "8,Godthaab / Nuuk Airport,Godthaab,Greenland,GOH,BGGH,64.19090271,-51.67810059,283,-3,E,America/Godthab,airport,OurAirports",
            "9,Kangerlussuaq Airport,Sondrestrom,Greenland,SFJ,BGSF,67.0122219,-50.71160316,165,-3,E,America/Godthab,airport,OurAirports",
            "10,Thule Air Base,Thule,Greenland,THU,BGTL,76.53119659,-68.70320129,251,-4,E,America/Thule,airport,OurAirports",
            "11,Akureyri Airport,Akureyri,Iceland,AEY,BIAR,65.66000366,-18.0727005,6,0,N,Atlantic/Reykjavik,airport,OurAirports",
            "12,Egilssta�ir Airport,Egilsstadir,Iceland,EGS,BIEG,65.28330231,-14.40139961,76,0,N,Atlantic/Reykjavik,airport,OurAirports",
            "13,Hornafj�r�ur Airport,Hofn,Iceland,HFN,BIHN,64.295601,-15.2272,24,0,N,Atlantic/Reykjavik,airport,OurAirports",
            "14,H�sav�k Airport,Husavik,Iceland,HZK,BIHU,65.952301,-17.426001,48,0,N,Atlantic/Reykjavik,airport,OurAirports",
            "15,�safj�r�ur Airport,Isafjordur,Iceland,IFJ,BIIS,66.05809784,-23.13529968,8,0,N,Atlantic/Reykjavik,airport,OurAirports",
            "16,Keflavik International Airport,Keflavik,Iceland,KEF,BIKF,63.98500061,-22.60560036,171,0,N,Atlantic/Reykjavik,airport,OurAirports",
            "17,Patreksfj�r�ur Airport,Patreksfjordur,Iceland,PFJ,BIPA,65.555801,-23.965,11,0,N,Atlantic/Reykjavik,airport,OurAirports",
            "18,Reykjavik Airport,Reykjavik,Iceland,RKV,BIRK,64.12999725,-21.94059944,48,0,N,Atlantic/Reykjavik,airport,OurAirports",
            "19,Siglufj�r�ur Airport,Siglufjordur,Iceland,SIJ,BISI,66.133301,-18.9167,10,0,N,Atlantic/Reykjavik,airport,OurAirports",
            "20,Vestmannaeyjar Airport,Vestmannaeyjar,Iceland,VEY,BIVM,63.42430115,-20.27890015,326,0,N,Atlantic/Reykjavik,airport,OurAirports"
        };

        var airports = new List<Airport>();
        var csvParserRegex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        for (int i = 0; i < rawData.Length; i++)
        {
            var values = csvParserRegex.Split(rawData[i]);

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

    private List<AirportDistance> GetAirportsByDistanceList()
    {
        var airports = GetAirports();
        var airportsByDistance = new List<AirportDistance>();

        foreach (var airport in airports)
        {
            airportsByDistance.Add(new AirportDistance
            {
                Id = airport.Id,
                Name = airport.Name,
                City = airport.City,
                Country = airport.Country,
                Latitude = airport.Latitude,
                Longitude = airport.Longitude
            });
        }

        return airportsByDistance;

    }
}