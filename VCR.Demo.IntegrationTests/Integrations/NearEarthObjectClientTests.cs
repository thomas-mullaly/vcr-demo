using FluentAssertions;
using VCR.Demo.Integrations;

namespace VCR.Demo.IntegrationTests.Integrations;

public class NearEarthObjectClientTests : VcrTestBase
{
    private readonly NearEarthObjectClient _client;

    public NearEarthObjectClientTests()
    {
        _client = new NearEarthObjectClient(CreateClient());
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
}