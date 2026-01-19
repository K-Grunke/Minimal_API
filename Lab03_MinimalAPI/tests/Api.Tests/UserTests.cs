using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Lab03_MinimalAPI.DTOs;

namespace Lab03_MinimalAPI.Tests;

public class UserTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UserTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Register_And_Get_User()
    {
        var register = new { Username = "john", Email = "john@test.com", Password = "Password123" };

        var post = await _client.PostAsJsonAsync("/api/v1/users/register", register);
        post.StatusCode.Should().Be(HttpStatusCode.Created);

        var get = await _client.GetAsync("/api/v1/users");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var users = await get.Content.ReadFromJsonAsync<List<UserDto>>();
        users!.Should().ContainSingle(u => u.Email == "john@test.com");
    }
}
