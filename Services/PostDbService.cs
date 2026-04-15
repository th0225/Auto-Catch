using AutoCatch.Data;
using AutoCatch.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoCatch.Service;

public class PostDbService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public PostDbService(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<SocialPost>> GetFavoritesAsync()
    {
        using var db = _dbFactory.CreateDbContext();
        return await db.Favorites.ToListAsync();
    }

    public async Task UpsertFavoriteAsync(SocialPost favorite)
    {
        using var db = _dbFactory.CreateDbContext();

        var isExist = await db.Favorites.FirstOrDefaultAsync(
            f => f.Id == favorite.Id);
        if (isExist == null)
        {
            db.Favorites.Add(favorite);
        }
        else
        {
            isExist = favorite;
            db.Favorites.Update(isExist);
        }

        await db.SaveChangesAsync();
    }

    public async Task RemoveFavoriteAsync(string id)
    {
        using var db = _dbFactory.CreateDbContext();

        var favorite = await db.Favorites.FindAsync(id);
        if (favorite != null)
        {
            db.Favorites.Remove(favorite);
            await db.SaveChangesAsync();
        }
    }
}