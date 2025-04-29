using equilog_backend.Common;
using equilog_backend.DTOs.AuthDTOs;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit.Abstractions;

namespace equilog_backend.Tests.Integration
{
    // Custom fixture to be reused for all tests.
    public class AuthTestsFixture : IDisposable
    {
        public CustomWebAppFactory Factory { get; }
        public HttpClient Client { get; }

        public AuthTestsFixture()
        {
            Factory = new CustomWebAppFactory();
            Client = Factory.CreateClient();
        }

        public void Dispose()
        {
            Client?.Dispose();
            Factory?.Dispose();
        }
    }

    public class AuthIntegrationTests : IClassFixture<AuthTestsFixture>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;
        private readonly CustomWebAppFactory _factory;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly List<string> _registeredUsers;

        // AUTH ENDPOINTS
        private readonly string _registerEndpoint = "/api/auth/register";
        private readonly string _loginEndpoint = "/api/auth/login";
        private readonly string _protectedEndpoint = "/api/horse";
        public AuthIntegrationTests(AuthTestsFixture fixture, ITestOutputHelper output)
        {
            _factory = fixture.Factory;
            _client = fixture.Client;
            _output = output;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            _registeredUsers = new List<string>();
        }

        private RegisterDto CreateUniqueTestUser(string prefix = "test")
        {
            string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
            string username = $"{prefix}_user_{uniqueId}";

            return new RegisterDto
            {
                UserName = username,
                Email = $"{username}@example.com",
                Password = "SecureP@ssw0rd!123",
                FirstName = $"Test{uniqueId}",
                LastName = $"User{uniqueId}",
                PhoneNumber = $"555{uniqueId.Substring(0, 7)}"
            };
        }

        private void LogResponse(string message, string content, HttpStatusCode statusCode)
        {
            _output.WriteLine($"{message} (Status: {statusCode})");
            _output.WriteLine(content);
        }

        [Fact]
        public async Task Register_Then_Login_Should_Return_Valid_Token()
        {
            // Arrange
            var registerDto = CreateUniqueTestUser();
            var loginDto = new LoginDto
            {
                Email = registerDto.Email,
                Password = registerDto.Password
            };

            // Act: Register
            var registerResponse = await _client.PostAsJsonAsync(_registerEndpoint, registerDto);
            var registerContent = await registerResponse.Content.ReadAsStringAsync();
            LogResponse("Register response JSON:", registerContent, registerResponse.StatusCode);

            // Assert: Registration successful.
            var response = JsonSerializer.Deserialize<ApiResponse<AuthResponseDto>>(
            registerContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(response);
            Assert.False(string.IsNullOrWhiteSpace(response?.Value?.AccessToken));

            // Act: Login with same credentials.
            var loginResponse = await _client.PostAsJsonAsync(_loginEndpoint, loginDto);
            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            LogResponse("Login response JSON:", loginContent, loginResponse.StatusCode);

            // Assert: Login successful.
            loginResponse.EnsureSuccessStatusCode();
            var loginResult = JsonSerializer.Deserialize<ApiResponse<AuthResponseDto>>(
                loginContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(loginResult);
            Assert.False(string.IsNullOrWhiteSpace(loginResult?.Value?.AccessToken));

            // Track for potential cleanup.
            _registeredUsers.Add(registerDto.Email);
        }

        [Fact]
        public async Task Login_With_Invalid_Password_Should_Fail()
        {
            // Arrange
            var registerDto = CreateUniqueTestUser("invalid");
            var loginDto = new LoginDto
            {
                Email = registerDto.Email,
                Password = "WrongPassword123!"
            };

            // Register first
            var registerResponse = await _client.PostAsJsonAsync(_registerEndpoint, registerDto);
            registerResponse.EnsureSuccessStatusCode();
            _registeredUsers.Add(registerDto.Email);

            // Act: Login with wrong password.
            var loginResponse = await _client.PostAsJsonAsync(_loginEndpoint, loginDto);
            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            LogResponse("Login failure response:", loginContent, loginResponse.StatusCode);

            // Assert - We're flexible about the exact error code.
            Assert.False(loginResponse.IsSuccessStatusCode,
                $"Expected login with wrong password to fail, but got success status {loginResponse.StatusCode}");
        }

        [Fact]
        public async Task Register_With_Same_Email_Should_Fail()
        {
            // Arrange
            var registerDto = CreateUniqueTestUser("duplicate");

            // Act: First registration.
            var firstResponse = await _client.PostAsJsonAsync(_registerEndpoint, registerDto);
            firstResponse.EnsureSuccessStatusCode();
            _registeredUsers.Add(registerDto.Email);

            // Act: Second registration with same email.
            var secondResponse = await _client.PostAsJsonAsync(_registerEndpoint, registerDto);
            var errorContent = await secondResponse.Content.ReadAsStringAsync();

            // Assert
            Assert.False(secondResponse.IsSuccessStatusCode,
                $"Expected duplicate registration to fail, but got success status {secondResponse.StatusCode}");
        }

        [Fact]
        public async Task Login_With_NonExisting_User_Should_Fail()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = $"nonexistent_{Guid.NewGuid():N}@example.com",
                Password = "AnyPassword123!"
            };

            // Act
            var loginResponse = await _client.PostAsJsonAsync(_loginEndpoint, loginDto);
            var errorContent = await loginResponse.Content.ReadAsStringAsync();
            LogResponse("Nonexistent user login response:", errorContent, loginResponse.StatusCode);

            // Assert - be more flexible about exact error code
            Assert.False(loginResponse.IsSuccessStatusCode,
                $"Expected login with nonexistent user to fail, but got success status {loginResponse.StatusCode}");
        }

        [Fact]
        public async Task Register_With_Invalid_Data_Should_Return_BadRequest()
        {
            // Arrange: User with invalid email.
            var invalidRegisterDto = new RegisterDto
            {
                UserName = "invalid_format_user",
                Email = "not-an-email",
                Password = "weak",
                FirstName = "Invalid",
                LastName = "Data"
            };

            // Act
            var response = await _client.PostAsJsonAsync(_registerEndpoint, invalidRegisterDto);
            var errorContent = await response.Content.ReadAsStringAsync();
            LogResponse("Invalid registration response:", errorContent, response.StatusCode);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Authenticated_Endpoint_Should_Require_Valid_Token()
        {

            // Register and get token.
            var registerDto = CreateUniqueTestUser("auth");

            // Register first
            var registerResponse = await _client.PostAsJsonAsync(_registerEndpoint, registerDto);
            registerResponse.EnsureSuccessStatusCode();
            var registerContent = await registerResponse.Content.ReadAsStringAsync();
            var authResult = JsonSerializer.Deserialize<ApiResponse<AuthResponseDto>>(
                registerContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            _registeredUsers.Add(registerDto.Email);

            // Act 1: Call protected endpoint without token.
            var unauthResponse = await _client.GetAsync(_protectedEndpoint);
            var unauthContent = await unauthResponse.Content.ReadAsStringAsync();
            LogResponse("Unauthenticated response:", unauthContent, unauthResponse.StatusCode);

            // Assert 1: Should be unauthorized.
            Assert.Equal(HttpStatusCode.Unauthorized, unauthResponse.StatusCode);

            // Act 2: Call protected endpoint with token.
            var authClient = _factory.CreateClient();
            authClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult?.Value?.AccessToken);
            var authResponse = await authClient.GetAsync(_protectedEndpoint);
            var authContent = await authResponse.Content.ReadAsStringAsync();
            LogResponse("Authenticated response:", authContent, authResponse.StatusCode);

            // We don't assert the exact status code here since it depends on what the endpoint does.
            // We only care that it's not Unauthorized (401).
            Assert.NotEqual(HttpStatusCode.Unauthorized, authResponse.StatusCode);
        }
    }
}