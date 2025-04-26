using System.Net;

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
        var response = await _client.GetAsync("api/horse");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

