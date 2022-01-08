using AutoMapper;

class AirportMappingProfile : Profile
{
    public AirportMappingProfile()
    {
        CreateMap<Airport, AirportDistance>();
    }
}