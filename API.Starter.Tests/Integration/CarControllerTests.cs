using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using API.Starter.Tests.Extensions;
using API.Starter.Tests.Fixtures;
using Business.Models.v1;
using inacs.v8.nuget.Core.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Persistence.Context.v1;
using Persistence.Entities.v1;
using Persistence.Interfaces.v1;
using Test.Api;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

namespace API.Starter.Tests.Integration;

public class CarControllerTests :
    IClassFixture<WebApplicationFactory<Startup>>,
    IClassFixture<LaunchSettingsFixture>,
    IDisposable
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Startup> _builder;

    public CarControllerTests(WebApplicationFactory<Startup> factory)
    {
        //Configure the DI container with a mock object
        _builder = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices((builderContext, serviceCollection) =>
            {
                serviceCollection.RemoveHostedServices();

                var actualContextOptions =
                    serviceCollection.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<ThingsContext>));
                serviceCollection.Remove(actualContextOptions!);
                
                SqliteConnection sqliteConnection = new SqliteConnection(builderContext.Configuration.GetConnectionString("TestDb"));
                sqliteConnection.Open();
                serviceCollection.AddDbContext<ThingsContext>(options =>
                    options.UseSqlite(sqliteConnection));
            });
        });

        using var scope = _builder.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<ThingsContext>()!;
        
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        _client = _builder.CreateClient();
    }

    //[Fact]
    //public async Task GetAll_givenTwoCars_WillReturnTwoCarResponses()
    //{
    //    using var scope = _builder.Services.CreateScope();
    //    var carRepository = scope.ServiceProvider.GetService<ICarRepository>()!;
    //    carRepository.Add(new Car
    //    {
    //        Make = "BMW",
    //        Model = "M3",
    //        YearOfCreation = 2005
    //    });
    //    carRepository.Add(new Car
    //    {
    //        Make = "Toyota",
    //        Model = "Corolla",
    //        YearOfCreation = 2017
    //    });
    //    await carRepository.SaveChangesAsync();
    //    var httpResponseMessage = await _client.GetAsync("Car");
    //    var body = await httpResponseMessage.Content.ReadAsStringAsync();
    //
    //    //Assert everything from here 
    //    Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
    //
    //    var responseBody = JsonConvert.DeserializeObject<ResponseContent<IList<CarResponse>>>(body)!;
    //    Assert.Equal(2, responseBody.Result.Count);
    //
    //    List<CarResponse> carResponses = responseBody.Result.OrderBy(x => x.Make).ToList();
    //    Assert.Equal("BMW", carResponses[0].Make);
    //    Assert.Equal("M3", carResponses[0].Model);
    //    Assert.Equal(2005, carResponses[0].YearOfCreation);
    //
    //    Assert.Equal("Toyota", carResponses[1].Make);
    //    Assert.Equal("Corolla", carResponses[1].Model);
    //    Assert.Equal(2017, carResponses[1].YearOfCreation);
    //}

    //[Fact]
    //public async Task GetAll_givenNoCars_WillReturnNoCarResponses()
    //{
    //    var httpResponseMessage = await _client.GetAsync("Car");
    //
    //    //Assert everything from here 
    //    Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
    //    var body = await httpResponseMessage.Content.ReadAsStringAsync();
    //
    //    var responseBody = JsonConvert.DeserializeObject<ResponseContent<IList<CarResponse>>>(body)!;
    //    Assert.Equal(0, responseBody.Result.Count);
    //}
    
    public void Dispose()
    {
        TestExtensions.ResetMetrics();
        using var scope = _builder.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<ThingsContext>()!;
        context.Database.EnsureDeleted();
        _client.Dispose();
    }
}