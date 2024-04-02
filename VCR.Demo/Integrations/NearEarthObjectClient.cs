using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using VCR.Demo.DTOs;

namespace VCR.Demo.Integrations;

public class NearEarthObjectClient : INearEarthObjectClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public NearEarthObjectClient(HttpClient httpClient, string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.nasa.gov/neo/rest/v1/");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<FeedResponse> Feed(DateOnly startDate, DateOnly endDate)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["start_date"] = startDate.ToString("yyyy-MM-dd");
        query["end_date"] = endDate.ToString("yyyy-MM-dd");
        query["api_key"] = _apiKey;

        var response = await _httpClient.GetAsync($"feed?{query}");
        var resultString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<FeedResponse>(resultString, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        });

        return result!;
    }
}