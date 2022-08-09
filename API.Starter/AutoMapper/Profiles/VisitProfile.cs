using AutoMapper;
using Business.Models.v1;
using inacs.v8.nuget.DevAttributes;
using Persistence.Entities.v1;

namespace Test.Api.AutoMapper.Profiles;

[Developer("Rosen Bozhkov", "rosen.bozhkov@itsoft.bg")]
internal class VisitProfile : Profile
{
    public VisitProfile()
    {
        CreateMap<VisitRegisterRequest, Visit>();
        CreateMap<Visit, VisitResponse>();
    }
}
