using VCR.Demo.DTOs;

namespace VCR.Demo.Integrations;

public interface INearEarthObjectClient
{
    Task<FeedResponse> Feed(DateOnly startDate, DateOnly endDate);
}