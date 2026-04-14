using IndexedDB.Blazor;
using AutoCatch.Models;

namespace AutoCatch.Services;

public class IndexedDbService
{
    private readonly IIndexedDbFactory _dbFactory;

    public IndexedDbService(IIndexedDbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<SocialPost>> GetFavoritesAsync()
    {
        using var db = await _dbFactory.Create<MyDbStore>();
        return [.. db.Favorites];
    }

    public async Task<bool> GetFavoriteExistAsync(string id)
    {
        using var db = await _dbFactory.Create<MyDbStore>();
        var exist = db.Favorites.FirstOrDefault(
            x => x.Id == id
        );

        return exist != null;
    }

    public async Task UpsertRecordsAsync(SocialPost socialPost)
    {
        using var db = await _dbFactory.Create<MyDbStore>();
        
        var exist = db.Favorites.FirstOrDefault(s => s.Id == socialPost.Id);
        if (exist == null)
        {
            db.Favorites.Add(socialPost);
        }
        else
        {
            db.Favorites.Remove(exist);
            db.Favorites.Add(socialPost);
        }

        await db.SaveChanges();
    }

    public async Task<bool> RemoveRecordsAsync(string id)
    {
        using var db = await _dbFactory.Create<MyDbStore>();

        var exist = db.Favorites.FirstOrDefault(x => x.Id == id);
        if (exist != null)
        {
            db.Favorites.Remove(exist);
            await db.SaveChanges();
            return true;
        }

        return false;
    }
}