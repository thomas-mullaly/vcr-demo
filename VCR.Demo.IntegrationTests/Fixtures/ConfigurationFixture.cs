using Microsoft.Extensions.Configuration;

namespace VCR.Demo.IntegrationTests.Fixtures;

public class ConfigurationFixture
{
    public IConfiguration Configuration { get; }

    public ConfigurationFixture()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}