using Api.Tests.BuilderCommands.CompanyCommands;
using Application.UseCases.Companies.Commands.CreateCompanyId;
using Domain.Entities;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Api.Tests
{
    public class CompanyApiTest
    {
        private readonly JsonSerializerOptions deserializeOptions;
        private readonly string route;
        
        public CompanyApiTest() 
        {
            deserializeOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            route = "api/v1/companies/";
        }

        [Fact]
        public async Task PostNewCompanyId_ShouldBeOk()
        {
            //Arrange
            var webApp = new ApiApp();
            var client = webApp.CreateClient();

            //Act
            var request = await client.PostAsJsonAsync(route, new object());

            //Assert
            request.EnsureSuccessStatusCode();
            var responseData = JsonSerializer.Deserialize<CreateCompanyIdDto>(await request.Content.ReadAsStringAsync(), deserializeOptions);
            Assert.True(responseData is not null);
            Assert.IsType<CreateCompanyIdDto>(responseData);
        }

        [Fact]
        public async Task PutNewCompany_ShouldBeOk()
        {
            //Arrange
            var webApp = new ApiApp();
            var client = webApp.CreateClient();
            var reponseCreateCompanyId = await client.PostAsJsonAsync(route, new object());
            var responseCompanyId = JsonSerializer.Deserialize<CreateCompanyIdDto>(await reponseCreateCompanyId.Content.ReadAsStringAsync(), deserializeOptions);
            var comercialSegmentList = await client.GetFromJsonAsync<List<CommercialSegment>>("/api/v1/commercial-segments");
            var company = new CreateCompanyCommandBuilder().WithCommercialSegmentId(comercialSegmentList!.First().Id).Build();
            
            //Act
            var requestPut = await client.PutAsJsonAsync(route+ responseCompanyId!.CompanyId, company);

            //Assert
            reponseCreateCompanyId.EnsureSuccessStatusCode();
            Assert.True(requestPut is not null);
        }
        
        [Fact]
        public async Task PatchNewCompany_ShouldBeOk()
        {
            //Arrange
            var webApp = new ApiApp();
            var client = webApp.CreateClient();

            //Act
            var request = await client.PostAsJsonAsync(route, new object());
            var responseCompanyId = JsonSerializer.Deserialize<CreateCompanyIdDto>(await request.Content.ReadAsStringAsync(), deserializeOptions);
            var companycommand = new UpdateCompanyCommandBuilder().Build();

            //Assert
            using StringContent jsonContent = new(JsonSerializer.Serialize(companycommand), Encoding.UTF8,"application/json");
            var requestPatch = await client.PatchAsync(route + responseCompanyId!.CompanyId, jsonContent);
            request.EnsureSuccessStatusCode();
            Assert.True(requestPatch is not null);
        }
    }
}
