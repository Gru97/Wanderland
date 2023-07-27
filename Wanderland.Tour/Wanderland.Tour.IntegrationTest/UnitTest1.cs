using Microsoft.AspNetCore.Mvc.Testing;

namespace Wanderland.Tour.IntegrationTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();
            var response = await httpClient.GetAsync($"Tour/{Guid.NewGuid()}/State");

        }
    }
}