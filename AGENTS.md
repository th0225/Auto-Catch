# AutoCatch - 開發者指南

## 專案概覽

- .NET 10 Blazor Server 應用程式，搭配 MudBlazor 9.3.0 UI
- 使用 Playwright (Chromium headless) 爬取 PTT 文章
- 單一專案解決方案：`AutoCatch.csproj`

## 指令

```
dotnet run              # 啟動開發伺服器
dotnet build            # 建置專案
```

尚未設定測試專案、CI、Linter 或格式化工具。

## 儲存機制 — 重要

- **所有資料儲存在瀏覽器的 localStorage**，而非資料庫。
  - 收藏清單鍵值：`"favorites"`（`SocialPost` JSON 列表）
  - 設定鍵值：`"ptt_settings"`（`PttSettings` JSON）
- **EF Core SQLite 套件有參考但未在執行時期使用。** `Program.cs` 沒有註冊 DbContext、沒有 `app.db`、也沒有遷移。若需要持久化儲存，請自行新增 — 不要假設 SQLite 已經設定好。

## 架構

```
Program.cs              # 程式入口；DI: HttpClient, MudBlazor, 服務註冊
Components/
  Pages/
    Home.razor          # 主頁：文章列表 + 爬取控制
    Favorites.razor     # 收藏文章頁面 (來自 localStorage)
    Settings.razor      # PTT 看板設定頁面 (來自 localStorage)
    NotFound.razor      # 404 頁面
Services/
  PttScraperService.cs  # Playwright 爬蟲 — 每次爬取都建立新的瀏覽器實例
  PostStateService.cs   # Singleton 全域狀態：PttPosts, IsLoading, CrawlCts
  PostDbService.cs      # localStorage 收藏清單 CRUD
  SettingDbService.cs   # localStorage 設定 CRUD
```

## 爬蟲機制

- `PttScraperService.CatchAllBoardsAsync()` 透過 `Task.WhenAll` 平行爬取多個看板
- 自動處理 PTT 年滿 18 歲驗證
- 過濾規則：最小推文數、Re: 回文、發錢文、公告/置底文
- 若文章數量不足，自動點擊「‹ 上頁」往前翻頁
- 支援 `CancellationToken` — 呼叫 `PostStateService.RequestStop()` 可中止爬取
- **每次爬取都會建立並銷毀完整的 Playwright+Chromium 實例**（非長駐重複使用）

## 狀態管理

- `PostStateService` 為 **Singleton** — 所有連線的使用者共用同一份文章列表
- `IsDateStale` 在最後一次爬取超過 15 分鐘後回傳 true
- 請勿註冊為 Scoped；已在 `Program.cs` 註冊為 Singleton

## 開發慣例

- 原始碼註解使用繁體中文；請保持一致。
- 已啟用 Nullable reference types 與 Implicit usings。
- csproj 已啟用 `BlazorDisableThrowNavigationException`。
- 404 頁面透過 `UseStatusCodePagesWithReExecute("/not-found")` 處理。
