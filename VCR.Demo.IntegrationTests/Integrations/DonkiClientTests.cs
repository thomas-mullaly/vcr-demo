using FluentAssertions;
using Microsoft.Extensions.Configuration;
using VCR.Demo.Integrations;
using VCR.Demo.IntegrationTests.Fixtures;

namespace VCR.Demo.IntegrationTests.Integrations;

public class DonkiClientTests : VcrTestBase, IClassFixture<ConfigurationFixture>
{
    private readonly DonkiClient _client;

    public DonkiClientTests(ConfigurationFixture fixture)
    {
        var apiKey = fixture.Configuration.GetValue<string>("DONKI_KEY")!;

        AddSecretsReplacement(apiKey, "API_KEY");
        _client = new DonkiClient(CreateClient(), apiKey);
    }

    [Fact]
    public async Task ListsSolarFlares()
    {
        using var cassette = UseCassette();

        var result = await _client.GetSolarFlares(new DateOnly(2016, 1, 1), new DateOnly(2016, 1, 30));

        result.Count.Should().BeGreaterThan(0);

        foreach (var solarFlare in result)
        {
            solarFlare.FlareID.Should().NotBeNull();
            solarFlare.ClassType.Should().NotBeNull();
        }
    }
}