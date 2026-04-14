using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor.Services;
using AutoCatch;
using AutoCatch.Services;
using IndexedDB.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// MudBlazor
builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<IIndexedDbFactory, IndexedDbFactory>();
builder.Services.AddScoped(sp => 
{
    var jsRuntime = sp.GetRequiredService<IJSRuntime>();
    return new MyDbStore(jsRuntime, "AutoCatchDB", 1);
});
builder.Services.AddScoped<IndexedDbService>();

await builder.Build().RunAsync();
