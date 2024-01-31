using System.Text.Json.Serialization;

namespace VCR.Demo.DTOs;

public class NearEarthObject
{
    public string Id { get; set; } = default!;

    public string NeoReferenceId { get; set; } = default!;

    public string Name { get; set; } = default!;

    public EstimatedDiameter EstimatedDiameter { get; set; } = default!;

    public List<CloseApproachData> CloseApproachData { get; set; } = default!;
}