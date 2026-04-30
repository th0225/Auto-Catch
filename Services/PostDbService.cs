using System.Text.Json;
using AutoCatch.Models;
using Microsoft.JSInterop;

namespace AutoCatch.Service;

public class PostDbService
{
    private readonly IJSRuntime _js;
    private const string StorageKey = "favorites";

    public PostDbService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<List<SocialPost>> GetFavoritesAsync()
    {
        try
        {
            var json = await _js.InvokeAsync<string>(
                "localStorage.getItem", StorageKey);
            if (string.IsNullOrWhiteSpace(json))
            {
                 return [];
            }

            return JsonSerializer.Deserialize<List<SocialPost>>(
                json, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            }) ?? [];
        }
        catch
        {
            return [];
        }
    }

    public async Task SaveFavoriteAsync(SocialPost post)
    {
        var favorites = await GetFavoritesAsync();

        if (favorites.Any(p => p.Id == post.Id)) return;

        favorites.Add(post);
        
        var json = JsonSerializer.Serialize(favorites);
        await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
    }

    public async Task RemoveFavoriteAsync(string id)
    {
        var favorites = await GetFavoritesAsync();
        
        if (favorites.Any())
        {
            var updated = favorites.Where(p => p.Id != id).ToList();
            
            if (updated.Count != favorites.Count)
            {
                var json = JsonSerializer.Serialize(updated);
                await _js.InvokeVoidAsync(
                    "localStorage.setItem", StorageKey, json);
            }
        }
    }

    public async Task ClearAllFavoritesAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
    }
}