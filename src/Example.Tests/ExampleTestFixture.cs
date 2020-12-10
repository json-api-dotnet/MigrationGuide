using System.Net.Http;
using System.Threading.Tasks;
using Example.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Example.Tests
{
    public abstract class ExampleTestFixture : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        protected ExampleTestFixture(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        protected async Task<HttpResponseMessage> ExecuteGetRequestAsync(string route)
        {
            var client = _factory.CreateClient();
            return await client.GetAsync(route);
        }
    }
}
