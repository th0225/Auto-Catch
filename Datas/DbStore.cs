using Microsoft.JSInterop;
using IndexedDB.Blazor;
using AutoCatch.Models;

public class MyDbStore : IndexedDb
{
    public MyDbStore(IJSRuntime jsRuntime, string name, int version) :
        base(jsRuntime, name: "AutoCatchDB", version: 1) {}
    public IndexedSet<SocialPost> Favorites { get; set; }
}