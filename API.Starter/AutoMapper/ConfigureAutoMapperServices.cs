using Test.Api.AutoMapper.Profiles;
using AutoMapper;
using inacs.v8.nuget.DevAttributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Test.Api.AutoMapper;

[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
internal static class ConfigureAutoMapperServices
{
    public static IMapper ConfigureAutomapper(this IServiceCollection services)
    {
        var config = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile(new CarProfile());
            configuration.AddProfile(new ModelProfile());
            configuration.AddProfile(new MakeProfile());
            configuration.AddProfile(new JobProfile());
            configuration.AddProfile(new VisitProfile());
            configuration.AddProfile(new UserProfile());
        });

        var mapper = config.CreateMapper();
        services.TryAddSingleton(mapper);
        return mapper;
    }
}