using VCR.Demo.DTOs;

namespace VCR.Demo.Integrations;

public interface IDonkiClient
{
    Task<List<SolarFlare>> GetSolarFlares(DateOnly startDate, DateOnly endDate);
}