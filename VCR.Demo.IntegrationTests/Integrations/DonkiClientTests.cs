using FluentAssertions;
using VCR.Demo.Integrations;

namespace VCR.Demo.IntegrationTests.Integrations;

public class DonkiClientTests : VcrTestBase
{
    private readonly DonkiClient _client;

    public DonkiClientTests()
    {
        _client = new DonkiClient(CreateClient(), "DEMO_KEY");
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