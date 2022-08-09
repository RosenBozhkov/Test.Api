using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;
using inacs.v8.nuget.Core.Models;
using inacs.v8.nuget.DevAttributes;
using inacs.v8.nuget.ExposeDeveloper.Models;

namespace Config.Swagger.Examples;

/// <summary>
/// A swagger filter that provides an example for <c cref="ResponseContent"/>
/// </summary>
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
public class ExposedDevelopersInformationExample : IExamplesProvider<ResponseContent<ExposedDevelopersInformation>>
{
    /// <summary>
    /// Gets the specified example
    /// </summary>
    /// <returns></returns>
    public ResponseContent<ExposedDevelopersInformation> GetExamples()
    {
        return new ResponseContent<ExposedDevelopersInformation>
        {
            Result = new ExposedDevelopersInformation
            {
                Assembly = new Dictionary<string, IList<DeveloperInformation>>
                {
                    {
                        "Api.Starter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                        new List<DeveloperInformation>
                        {
                            new() { Name = "Daniel", Email = "daniel.tanev@itsoft.bg" },
                            new() { Name = "Vasil", Email = "vegov@itsoft.bg" }
                        }
                    }
                },
                Type = new Dictionary<string, IList<DeveloperInformation>>
                {
                    {
                        "Api.Starter.Program", new List<DeveloperInformation>
                        {
                            new() { Name = "Daniel", Email = "daniel.tanev@itsoft.bg" },
                        }
                    },
                    {
                        "Api.Starter.Startup", new List<DeveloperInformation>
                        {
                            new() { Name = "Daniel", Email = "daniel.tanev@itsoft.bg" },
                            new() { Name = "Vasil", Email = "vegov@itsoft.bg" }
                        }
                    },
                },
                Method = new Dictionary<string, MethodInformation>
                {
                    {
                        "Api.Starter.Controllers.v1.WeatherForecastController.GetWeather()", new MethodInformation
                        {
                            Routes = new List<RouteInfo>
                            {
                                new()
                                {
                                    Route = "/hello/world",
                                    Methods = new[] { "GET" }
                                }
                            },
                            DeveloperInformation = new List<DeveloperInformation>
                            {
                                new()
                                {
                                    Name = "Daniel",
                                    Email = "daniel.tanev@itsoft.bg"
                                }
                            }
                        }
                    },
                    {
                        "Api.Starter.Controllers.v1.WeatherForecastController.UpdateWeather()",
                        new MethodInformation
                        {
                            Routes = new List<RouteInfo>
                            {
                                new()
                                {
                                    Route = "/first/route",
                                    Methods = new[] { "GET" }
                                },
                                new()
                                {
                                    Route = "/some/other/route",
                                    Methods = new[] { "POST" }
                                },
                            },
                            DeveloperInformation = new List<DeveloperInformation>
                            {
                                new()
                                {
                                    Name = "Daniel",
                                    Email = "daniel.tanev@itsoft.bg"
                                },
                                new()
                                {
                                    Name = "Vasil",
                                    Email = "vegov@itsoft.bg"
                                }
                            }
                        }
                    }
                }
            },
            Meta = new Meta
            {
                RequestId = "d6dc1d737d5d264abb0bdb762c3ffa13",
                ExecutionTimeMs = 156,
                ExecutionFinishedUTC = new System.DateTime(2021, 6, 24, 9, 57, 36, 110),
                HttpMethod = "POST",
                Path = "/dog/10",
                Version = "2.0",
                InstanceId = 3,
                HealthEndpoint = "/health"
            }
        };
    }
}