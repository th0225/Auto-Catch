using System.ComponentModel.DataAnnotations;

namespace AutoCatch.Models;

public class UserSettings
{
    [Key]
    public int Id { get; set; } = 0;
    // 關鍵字清單
    public List<string> Keywords { get; set; } = [];
    
    public bool EnableThreads { get; set; } = true;
    public bool EnablePtt { get; set; } = false;
    public bool EnableX { get; set; } = false;
    
    // 更新時間
    public int RefreshIntervalMinutes { get; set; } = 30;
}