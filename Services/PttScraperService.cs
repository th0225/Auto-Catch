using AutoCatch.Models;
using Microsoft.Playwright;

public class PttScraperService
{
    public async Task<List<SocialPost>> GetPttPosts(
        string board, int targetCount = 30)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(
            new() { Headless = true}
        );
        var page = await browser.NewPageAsync();

        List<SocialPost> allPosts = [];
        await page.GotoAsync($"https://www.ptt.cc/bbs/{board}/index.html");

        // 處理18歲驗證
        var over18Btn = await page.QuerySelectorAsync("button[name='yes']");
        if (over18Btn != null)
        {
            await over18Btn.ClickAsync();
        }

        while (allPosts.Count < targetCount)
        {
            // 等待文章列表載入
            await page.WaitForSelectorAsync(".r-ent");

            // 抓取當前頁面的所有文章節點
            var postNodes = await page.QuerySelectorAllAsync(".r-ent");

            foreach (var node in postNodes)
            {
                var titleElement = await node.QuerySelectorAsync(".title a");
                if (titleElement == null)
                {
                    continue;
                }

                var title = await titleElement.InnerTextAsync();
                if (title.Contains("[公告]"))
                {
                    continue;
                }

                var url = await titleElement.GetAttributeAsync("href");
                var author = await (
                    await node.QuerySelectorAsync(".author")
                ).InnerTextAsync();
                var dateStr = await (
                    await node.QuerySelectorAsync(".date")
                ).InnerTextAsync();

                allPosts.Add(new SocialPost
                {
                    Content = title,
                    Url = "https://www.ptt.cc" + url,
                    Author = author,
                    Date = ParsePttDate(dateStr),
                    Platform = "PTT"
                });

                if (allPosts.Count >= targetCount)
                {
                    break;
                }
            }

            // 數量不夠時，點擊上一頁往回追溯
            if (allPosts.Count < targetCount)
            {
                var prevPageBtn = page.GetByText("‹ 上頁").First;
                if (prevPageBtn != null)
                {
                    await prevPageBtn.ClickAsync();
                    await page.WaitForTimeoutAsync(1000);
                }
                else
                {
                    break;
                }
            }
        }

        await browser.CloseAsync();
        return allPosts;
    }

    private DateTime ParsePttDate(string dateStr)
    {
        try {
            return DateTime.Parse($"{DateTime.Now.Year}/{dateStr.Trim()}");
        } catch {
            return DateTime.Now;
        }
    }
}