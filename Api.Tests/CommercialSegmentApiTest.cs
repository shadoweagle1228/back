using System.Net;
using Api.Tests.BuilderCommands.CommercialSegmentCommands;
using Application.UseCases.ComercialSegments.Commands.CreateCommercialSegmentId;
using Application.UseCases.ComercialSegments.Queries.GetAllCommercialSegment;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Domain.Entities;
using Domain.Ports;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests;

public class CommercialSegmentApiTest : IClassFixture<ApiApp>
{
    private readonly JsonSerializerOptions deserializeOptions;
    private readonly string route;
    private readonly HttpClient client;
    private readonly ApiApp _apiApp;

    public CommercialSegmentApiTest(ApiApp apiApp)
    {
        client = apiApp.CreateClient();
        _apiApp = apiApp;
        deserializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        route = "api/v1/commercial-segments/";
    }
    
    [Fact]
    public async Task PostNewCommercialSegmentId_ShouldBeOk()
    {
        //Arrange - Act
        var request = await client.PostAsJsonAsync(route, new object());

        //Assert
        request.EnsureSuccessStatusCode();
        var responseData = JsonSerializer.Deserialize<CreateCommercialSegmentIdDto>(await request.Content.ReadAsStringAsync(), deserializeOptions);
        Assert.True(responseData is not null);
        Assert.IsType<CreateCommercialSegmentIdDto>(responseData);
    }

    [Fact]
    public async Task PutNewCompany_ShouldBeOk()
    {
        //Arrange - Act
        var request = await client.PostAsJsonAsync(route, new object());
            
        var responseId = JsonSerializer.Deserialize<CreateCommercialSegmentIdDto>(await request.Content.ReadAsStringAsync(), deserializeOptions);
        var commercialSegment = new CreateCommercialSegmentCommandBuilder().WithId(responseId!.CommercialSegmentId).Build();

        //Assert
        var requestPut = await client.PutAsJsonAsync(route + commercialSegment.Id, commercialSegment);
        request.EnsureSuccessStatusCode();
        Assert.True(requestPut is not null);
    }
    
    [Fact]
    public async Task PathcNewCompany_ShouldBeOk()
    {
        //Arrange - Act
        var request = await client.PostAsJsonAsync(route, new object());
        var responseId = JsonSerializer.Deserialize<CreateCommercialSegmentIdDto>(await request.Content.ReadAsStringAsync(), deserializeOptions);
        var commercialSegment = new UpdateCommercialSegmentCommandBuilder().WithId(responseId!.CommercialSegmentId).Build();
        using StringContent jsonContent = new(JsonSerializer.Serialize(commercialSegment), Encoding.UTF8, "application/json");
        var requestPatch = await client.PatchAsync(route + commercialSegment.Id, jsonContent);

        //Assert
        request.EnsureSuccessStatusCode();
        Assert.True(requestPatch is not null);
    }

    [Fact]
    public async Task DeleteCommercialSegment_ShouldBeOk()
    {
        //Arrange
        var serviceCollection = _apiApp.GetServiceCollection();
        using var scope = serviceCollection.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService< IGenericRepository<CommercialSegment>>();
        var commercialSegment = (await repository.GetAsync()).FirstOrDefault();
        
        //Act
        var requestDelete = await client.DeleteAsync(route + commercialSegment!.Id);

        //Assert
        requestDelete.EnsureSuccessStatusCode();
    }
        
    [Fact]
    public async Task DeleteCommercialSegmentWhenNotExist_ShouldBeNotFound()
    {
        //Arrange - Act
        var response = await client.DeleteAsync(route + Guid.NewGuid());

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAllCommercialSegment_ShouldBeOk()
    {
        //Arrange
        await using var webApp = new ApiApp();
        var client = webApp.CreateClient();

        //Act
        var comercialSegmentList = await client.GetFromJsonAsync<List<GetAllCommercialSegmentDto>>(route);

        //assert
        Assert.True(comercialSegmentList != null);
        Assert.IsType<List<GetAllCommercialSegmentDto>>(comercialSegmentList);
          
    }

       
}