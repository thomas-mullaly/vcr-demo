using System.Text.Json.Serialization;

namespace VCR.Demo.DTOs;

public class SolarFlare
{
    [JsonPropertyName("flrID")]
    public string FlareID { get; set; } = default!;
    
    [JsonPropertyName("beginTime")]
    public DateTime BeginTime { get; set; }
    
    [JsonPropertyName("peakTime")]
    public DateTime? PeakTime { get; set; }
    
    [JsonPropertyName("endTime")]
    public DateTime? EndTime { get; set; }
    
    [JsonPropertyName("classType")]
    public string ClassType { get; set; }
    
    [JsonPropertyName("sourceLocation")]
    public string SourceLocation { get; set; }
}