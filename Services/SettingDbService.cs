using AutoCatch.Data;
using AutoCatch.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoCatch.Service;

public class SettingDbService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public SettingDbService(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<PttSettings> GetPttSettingsAsync()
    {
        using var db = _dbFactory.CreateDbContext();
        var settings = await db.PttSettings.FirstOrDefaultAsync();

        if (settings == null)
        {
            return new PttSettings
            {
                Enabled = true,
                Boards = ["Lifeismoney"],
                NumPost = 10,
                MinNrec = 0,
                RefreshIntervalMinutes = 30
            };
        }
        else
        {
            return settings;
        }
    }

    public async Task UpdatePttSettingsAsync(PttSettings settings) =>
        await UpdateSettingsAsync(settings);

    public async Task<ThreadsSettings> GetThreadsSettingsAsync()
    {
        using var db = _dbFactory.CreateDbContext();
        var settings = await db.ThreadsSettings.FirstOrDefaultAsync();

        if (settings == null)
        {
            return new ThreadsSettings
            {
                Enabled = true,
                Keywords = ["AI"],
                NumPost = 10,
                MinLikes = 0,
                RefreshIntervalMinutes = 30
            };
        }
        else
        {
            return settings;
        }
    }

    public async Task UpdateThreadsSettingsAsync(ThreadsSettings settings) =>
        await UpdateSettingsAsync(settings);

    private async Task UpdateSettingsAsync<T>(T settings) where T : class
    {
        using var db = _dbFactory.CreateDbContext();
        db.Set<T>().Update(settings);
        await db.SaveChangesAsync();
    }
}