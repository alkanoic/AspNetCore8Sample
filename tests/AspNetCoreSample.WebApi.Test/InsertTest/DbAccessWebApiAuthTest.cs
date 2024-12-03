using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using AspNetCoreSample.WebApi.Services.Keycloak.Token;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCoreSample.WebApi.Test;

public sealed class DbAccessWebApiAuthTest : IClassFixture<KeycloakFixture>, IClassFixture<DbFixture>, IDisposable
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private readonly IServiceScope _serviceScope;
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

    public DbAccessWebApiAuthTest(KeycloakFixture keycloak, DbFixture db)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_URLS", "https://+");
        // Environment.SetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path", "certificate.crt");
        // Environment.SetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password", "password");
        Environment.SetEnvironmentVariable("ConnectionStrings__Default", db.DbConnectionString);
        Environment.SetEnvironmentVariable("KeycloakOptions__TokenEndpoint", $"{keycloak.BaseAddress}realms/Test/protocol/openid-connect/token");
        Environment.SetEnvironmentVariable("KeycloakOptions__Authority", $"{keycloak.BaseAddress}realms/Test");
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        _webApplicationFactory = new WebApplicationFactory<Program>();
        _serviceScope = _webApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        _httpClient = _webApplicationFactory.CreateClient();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        _serviceScope.Dispose();
        _webApplicationFactory.Dispose();
    }

    [Fact]
    [Trait("Category", nameof(DbAccessWebApiAuthTest))]
    public async Task PostDbAccessAuth()
    {
        // Given
        const string path = "api/token/auth";

        // When
        var content = new StringContent(JsonSerializer.Serialize(new { userName = "admin", password = "admin" }, JsonSerializerOptions), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(path, content);
        var dbAccessStream = await response.Content.ReadAsStreamAsync();
        var tokenResponse = await JsonSerializer.DeserializeAsync<TokenResponse>(dbAccessStream, JsonSerializerOptions);

        // Then
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(tokenResponse);
        Assert.NotEmpty(tokenResponse.AccessToken);
        Assert.NotEmpty(tokenResponse.RefreshToken);

        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
        var test_response = await _httpClient.GetAsync("api/TokenTest/SampleAdmin?sample=asd");

        var stream = await test_response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, test_response.StatusCode);
        Assert.NotEmpty(stream);
    }
}
