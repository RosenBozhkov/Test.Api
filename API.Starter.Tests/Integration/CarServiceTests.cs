using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Starter.Tests.Fixtures;
using Business.Interfaces.v1;
using inacs.v8.nuget.Telemetry.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;
using Xunit;

namespace API.Starter.Tests.Integration;

/// <summary>
/// Integration test showing how to inject mocks into container without starting an entire host.
/// </summary>
public class CarServiceTests :
    IClassFixture<LaunchSettingsFixture>,
    IClassFixture<DependencyContainerFixture>
{
    private readonly Mock<ICarRepository> _carRepository = new();
    private readonly Mock<ITelemetryProvider> _telemetryProvider = new();

    private readonly ServiceCollection _serviceCollection;

    public CarServiceTests(DependencyContainerFixture fixture)
    {
        _serviceCollection = fixture.ServiceCollection;
    }

    //[Fact]
    //public async Task GetAll_GivenOneCar_WillReturnOneCarResponse()
    //{
    //    _serviceCollection.AddScoped(_ => _carRepository.Object);
    //    _serviceCollection.AddScoped(_ => _telemetryProvider.Object);
    //    _carRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Car>
    //    {
    //        new()
    //        {
    //            Make = "BMW",
    //            Model = "M3",
    //            YearOfCreation = 2005
    //        }
    //    });
    //    _telemetryProvider.Setup(x => x.GetTraceId()).Returns(Guid.NewGuid);
    //
    //    var sp = _serviceCollection.BuildServiceProvider();
    //    var carService = sp.GetService<ICarService>()!;
    //
    //    var actualCars = await carService.GetAllAsync();
    //    
    //    Assert.Equal(1, actualCars.Count);
    //
    //    Assert.Equal("BMW", actualCars[0].Make);
    //    Assert.Equal("M3", actualCars[0].Model);
    //    Assert.Equal(2005, actualCars[0].YearOfCreation);
    //}
}