# 🍦 AutoCatch - Ptt自動追蹤器

AutoCatch 是為了快速追蹤PTT感興趣看板文章的工具，採用Blazor Server開發，並可儲存喜愛的文章。

---

## ✨ 特色功能

- **奶油玻璃質感 (Glassmorphism)**：全介面採用半透明玻璃濾鏡與柔和圓角設計，完美適應深色模式 (Dark Mode)。
- **極簡本地儲存 (LocalStorage)**：
    - **零伺服器負擔**：使用瀏覽器 LocalStorage 儲存收藏清單，確保隱私並減輕 SQLite 寫入壓力。
    - **跨 session 記憶**：重新整理或重啟瀏覽器後，收藏的文章與狀態依然存在。
- **高效能 PTT 爬蟲**：
    - 基於 **Playwright** 驅動，支援動態網頁抓取。
    - 支援自定義看板（上限 3 個）、抓取篇數、最低推文門檻。
    - 進階過濾：自動排除「Re:」回文與「發錢」文。
- **全域狀態同步**：採用 Singleton 狀態管理，確保不同分頁間的數據即時同步，無需重複抓取。

---

## 🛠 技術架構

- **Framework**: .NET 10 (Blazor Server)
- **UI Library**: MudBlazor
- **Automation**: Playwright (Chromium)
- **Storage**: 
  - **SQLite**: 儲存使用者設定與爬蟲配置。
  - **LocalStorage**: 儲存文章收藏 ID 與內容。

---
