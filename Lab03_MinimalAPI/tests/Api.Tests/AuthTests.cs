using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace Lab03_MinimalAPI.Tests;

public class AuthTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Login_And_Access_Protected_Endpoint()
    {
        // Rejestracja
        var register = new { Username = "anna", Email = "anna@test.com", Password = "Password123" };
        await _client.PostAsJsonAsync("/api/v1/users/register", register);

        // Logowanie po Username, a nie po Email
        var login = await _client.PostAsJsonAsync("/api/v1/users/login",
            new { Username = "anna", Password = "Password123" });

        login.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginResponse = await login.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        var token = loginResponse!["token"];
        token.Should().NotBeNullOrEmpty();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Próba utworzenia zadania (chroniony endpoint)
        var task = new { Title = "Test task", Description = "Task 1" };
        var response = await _client.PostAsJsonAsync("/api/v1/tasks", task);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
