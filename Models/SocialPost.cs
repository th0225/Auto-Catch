namespace AutoCatch.Models;

public class SocialPost
{
    [System.ComponentModel.DataAnnotations.Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Platform { get; set; } = "Threads";
    public string Author { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CapturedAt { get; set; } = DateTime.Now;
    public string Url { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
}