using FluentAssertions;
using Microsoft.Extensions.Configuration;
using VCR.Demo.Integrations;
using VCR.Demo.IntegrationTests.Fixtures;

namespace VCR.Demo.IntegrationTests.Integrations;

public class NearEarthObjectClientTests : VcrTestBase, IClassFixture<ConfigurationFixture>
{
    private readonly NearEarthObjectClient _client;

    public NearEarthObjectClientTests(ConfigurationFixture fixture)
    {
        var apiKey = fixture.Configuration.GetValue<string>("NEO_KEY")!;
        
        AddSecretsReplacement(apiKey, "NEO_KEY");

        _client = new NearEarthObjectClient(CreateClient(), apiKey);
    }

    [Fact]
    public async Task Test()
    {
        using var cassette = UseCassette();

        var results = await _client.Feed(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 7));

        results.NearEarthObjects.Count.Should().BeGreaterThan(0);

        foreach (var neo in results.NearEarthObjects)
        {
            neo.Key.Should().NotBeNullOrEmpty();

            neo.Value.Count.Should().BeGreaterThan(0);
        }
    }

    [Fact]
    public async Task MultipleInteractions()
    {
        using var cassette = UseCassette();

        var results = await _client.Feed(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 7));
        results = await _client.Feed(new DateOnly(2024, 1, 8), new DateOnly(2024, 1, 10));
    }
}