using System.ComponentModel.DataAnnotations;

namespace AutoCatch.Models;

public class PttSettings
{
    [Key]
    public int Id { get; set; } = 0;
    
    // 是否啟用
    public bool Enabled{ get; set; } = false;
    // 抓取的看版
    public List<string> Boards { get; set; } = [];
    // 總共抓取的文章數量
    public int NumPost = 10;
    // 推文數
    public int MinNrec { get; set; } = 0;

    // 更新時間(分)
    public int RefreshIntervalMinutes { get; set; } = 30;
}

public class ThreadsSettings
{
    [Key]
    public int Id { get; set; } = 0;

    // 是否啟狦
    public bool Enabled { get; set; } = false;
    // 抓取的關鍵字
    public List<string> Keywords { get; set; } = [];
    // 總共抓取的文章數量
    public int NumPost = 10;
    // 愛心數
    public int MinLikes { get; set; } = 0;

    // 更新時間(分)
    public int RefreshIntervalMinutes { get; set; } = 30;
}