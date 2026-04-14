namespace AutoCatch.Models;

public class UserSettings
{
    public int Id { get; set; } = 0;
    public List<string> Keywords { get; set; } = [];
    public bool EnableThreads { get; set; } = true;
    public bool EnablePtt { get; set; } = false;
    public bool EnableX { get; set; } = false;
    public int RefreshIntervalMinutes { get; set; } = 30;
}