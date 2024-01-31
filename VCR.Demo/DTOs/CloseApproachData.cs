namespace VCR.Demo.DTOs;

public class CloseApproachData
{
    public DateOnly CloseApproachDate { get; set; }
    public RelativeVelocity RelativeVelocity { get; set; } = default!;
    public MissDistance MissDistance { get; set; } = default!;
}