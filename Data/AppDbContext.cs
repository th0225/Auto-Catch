using Microsoft.EntityFrameworkCore;
using AutoCatch.Models;
using System.Text.Json;

namespace AutoCatch.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :
        base(options) {}

    public DbSet<SocialPost> Favorites => Set<SocialPost>();
    public DbSet<PttSettings> PttSettings => Set<PttSettings>();
    public DbSet<ThreadsSettings> ThreadsSettings => Set<ThreadsSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PttSettings>()
            .Property(e => e.BoardConfigs)
            .HasConversion(
                v => JsonSerializer.Serialize(
                    v, (JsonSerializerOptions)null
                ),
                v => JsonSerializer.Deserialize<List<PttBoardConfig>>(
                    v, (JsonSerializerOptions)null) ?? new List<PttBoardConfig>()
            );
        
        modelBuilder.Entity<ThreadsSettings>()
            .Property(e => e.Keywords)
            .HasConversion(
                v => JsonSerializer.Serialize(
                    v, (JsonSerializerOptions)null
                ),
                v => JsonSerializer.Deserialize<List<string>>(
                    v, (JsonSerializerOptions)null)
                    ?? new List<string>()
            );

        modelBuilder.Entity<PttSettings>().HasData(
            new PttSettings
            {
                Id = 1,
                Enabled = true,
                RefreshIntervalMinutes = 30,
                BoardConfigs =
                [
                    new PttBoardConfig
                    {
                        Name = "Lifeismoney",
                        NumPost = 10,
                        MinNrec = 0,
                        HideReplies = true
                    }
                ]
            }
        );

        modelBuilder.Entity<ThreadsSettings>().HasData(
            new ThreadsSettings
            {
                Id = 1,
                Enabled = true,
                Keywords = ["AI"],
                NumPost = 10,
                MinLikes = 0,
                RefreshIntervalMinutes = 30
            }
        );
    }
}