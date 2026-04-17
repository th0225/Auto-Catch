using AutoCatch.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace AutoCatch.Service;

public class SettingDbService
{
    private readonly IJSRuntime _js;
    public SettingDbService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<PttSettings> GetPttSettingsAsync()
    {
        var json = await _js.InvokeAsync<string>(
            "localStorage.getItem", "ptt_settings"
        );

        if (string.IsNullOrEmpty(json))
        {
            return new PttSettings();
        }
        else
        {
            return JsonSerializer.Deserialize<PttSettings>(json)!;
        }
    }

    public async Task SavePttSettingsAsync(PttSettings pttSettings)
    {
        var json = JsonSerializer.Serialize(pttSettings);
        await _js.InvokeVoidAsync(
            "localStorage.setItem", "ptt_settings", json
        );
    }
}