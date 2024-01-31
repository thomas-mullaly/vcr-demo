namespace VCR.Demo.DTOs;

public class FeedResponse
{
    public Dictionary<string, List<NearEarthObject>> NearEarthObjects { get; set; } = new();
}