using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Web;
using VCR.Demo.DTOs;

namespace VCR.Demo.Integrations;

public class DonkiClient : IDonkiClient
{
    private readonly HttpClient _client;
    private readonly string _apiKey;

    public DonkiClient(HttpClient client, string apiKey)
    {
        _client = client;
        _apiKey = apiKey;

        _client.BaseAddress = new Uri("https://api.nasa.gov/DONKI/");
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<List<SolarFlare>> GetSolarFlares(DateOnly startDate, DateOnly endDate)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        
        query["startDate"] = startDate.ToString("yyyy-MM-dd");
        query["endDate"] = endDate.ToString("yyyy-MM-dd");
        query["api_key"] = _apiKey;

        var result = await _client.GetFromJsonAsync<List<SolarFlare>>($"FLR?{query}");

        return result!;
    }
}