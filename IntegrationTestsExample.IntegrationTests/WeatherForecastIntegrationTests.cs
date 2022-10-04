using IntegrationTestsExample.Api;
using IntegrationTestsExample.IntegrationTests.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTestsExample.IntegrationTests
{
    public class WeatherForecastIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public WeatherForecastIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task HealthCheck_ShouldReturnOk()
        {
            var client = _factory.CreateClient();
            var response = await client.GetStringAsync("/healthcheck");

            Assert.Equal(response, "Healthy", StringComparer.InvariantCultureIgnoreCase);
        }

        [Fact]
        public async Task GetAll_ShouldReturn_AllItems()
        {
            var expected = new string[] { "Bracing", "Freezing", "Cool", "Chilly", "Mild" };

            var client = _factory.CreateClient();
            var response = await client.GetFromJsonAsync<List<WeatherForecastTestModel>>("/WeatherForecast");

            var orderedResponse = response.Select(x => x.Summary).OrderBy(x => x);
            var orderedExpected = expected.ToList().OrderBy(x => x);

            var equal = orderedExpected.SequenceEqual(orderedResponse);
            
            Assert.True(equal);
        }
    }
}