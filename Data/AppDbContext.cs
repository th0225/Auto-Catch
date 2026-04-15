using Microsoft.EntityFrameworkCore;
using AutoCatch.Models;

namespace AutoCatch.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :
        base(options) {}

    public DbSet<SocialPost> Favorites => Set<SocialPost>();
    public DbSet<UserSettings> UserSettings => Set<UserSettings>();
}