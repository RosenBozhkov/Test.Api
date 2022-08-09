using AutoMapper;
using Business.Models.v1;
using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.v1;

namespace Test.Api.AutoMapper.Profiles;

[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
internal class CarProfile : Profile
{
    public CarProfile()
    {
        CreateMap<CarRequest, Car>();
        CreateMap<Car, CarResponse>();
    }
}