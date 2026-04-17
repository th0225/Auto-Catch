using AutoCatch.Models;

namespace AutoCatch.Service;

public class PostStateService
{
    public List<SocialPost> PttPosts { get; set; } = [];
    public DateTime? LastPttUpdateTime { get; set; }
    public bool IsLoading { get; set; } = false;
    public bool IsDateStale => !LastPttUpdateTime.HasValue ||
        DateTime.Now.Subtract(LastPttUpdateTime.Value).TotalMinutes > 15;
    public CancellationTokenSource? CrawlCts { get; set; }

    public void RequestStop()
    {
        CrawlCts?.Cancel();
    }
}