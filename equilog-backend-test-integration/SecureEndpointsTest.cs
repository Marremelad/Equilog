namespace equilog_backend_test_integration;

public class SecuredEndpointsTests
{
    private readonly HttpClient _client;

    public SecuredEndpointsTests()
    {
        var factory = new CustomWebAppFactory(useFakeAuth: true);
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Can_Access_Secured_Endpoint()
    {
        var response = await _client.GetAsync("api/horse");
        response.EnsureSuccessStatusCode();
    }
}

