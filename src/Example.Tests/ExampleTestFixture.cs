using Example.Api.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Example.Tests;

public abstract class ExampleTestFixture : IClassFixture<WebApplicationFactory<AppDbContext>>
{
    private readonly WebApplicationFactory<AppDbContext> _factory;

    protected ExampleTestFixture(WebApplicationFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    protected async Task<HttpResponseMessage> ExecuteGetRequestAsync(string route)
    {
        HttpClient client = _factory.CreateClient();
        return await client.GetAsync(route);
    }
}
