using System.ComponentModel.DataAnnotations;

namespace AutoCatch.Models;

public class SocialPost
{
    [Key]
    public string Id { get; set; } = string.Empty;
    // 社群平台
    public string Platform { get; set; } = "Threads";
    // 作者
    public string Author { get; set; } = string.Empty;
    // 內容
    public string Content { get; set; } = string.Empty;
    // 時間
    public DateTime Date{ get; set; } = DateTime.Now;
    // 網址
    public string Url { get; set; } = string.Empty;
    // 
    public List<string> Tags { get; set; } = [];
    // 是否收藏
    public bool IsFavorite { get; set; } = false;
}