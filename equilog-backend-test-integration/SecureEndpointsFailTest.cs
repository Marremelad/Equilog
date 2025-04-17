using equilog_backend.DTOs.HorseDTOs;
using System.Net;
using System.Net.Http.Json;

namespace equilog_backend_test_integration;

public class SecuredEndpointsFailTests
{
    private readonly HttpClient _client;

    public SecuredEndpointsFailTests()
    {
        var factory = new CustomWebAppFactory(useFakeAuth: false);
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Cant_Access_Secured_Endpoint()
    {
        var horseDto = new HorseCreateDto
        {
            Name = "Comet",
            Color = "Brown",
            Breed = "Arabian",
            Age = new DateOnly(2018, 1, 1)
        };
        var response = await _client.PostAsJsonAsync("api/horse/create", horseDto);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

