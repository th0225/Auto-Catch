using AutoCatch.Models;
using Microsoft.Playwright;

public class PttScraperService
{
    public async Task<List<SocialPost>> GetPostsFromBoardAsync(
        string board, int targetCount, int minPush, bool hideReplies)
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
                var nrecElement = await node.QuerySelectorAsync(".nrec");
                var nrecText = nrecElement != null ?
                    await nrecElement.InnerTextAsync() : "";
                int pushCount = ParsePushCount(nrecText);

                // 略過最小推文數
                if (pushCount < minPush)
                {
                    continue;
                }

                // 略過無標題文章
                var titleElement = await node.QuerySelectorAsync(".title a");
                if (titleElement == null)
                {
                    continue;
                }

                // 略過公告、置底
                var title = await titleElement.InnerTextAsync();
                if (title.Contains("[公告]") || title.Contains("[置底]"))
                {
                    continue;
                }

                // 略過回文
                if (hideReplies &&
                    title.StartsWith("Re:", StringComparison.OrdinalIgnoreCase))
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
                    Platform = "PTT",
                    Tags = [board]
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

    public async Task<List<SocialPost>> CatchAllBoardsAsync(
        List<PttBoardConfig> configs)
    {
        var tasks = configs.Select(async config =>
        {
            return await GetPostsFromBoardAsync(
                config.Name,
                config.NumPost,
                config.MinNrec,
                config.HideReplies
            );
        });

        var results = await Task.WhenAll(tasks);

        return [.. results.SelectMany(x => x).OrderByDescending(p => p.Date)];
    }

    private DateTime ParsePttDate(string dateStr)
    {
        try {
            return DateTime.Parse($"{DateTime.Now.Year}/{dateStr.Trim()}");
        } catch {
            return DateTime.Now;
        }
    }

    private int ParsePushCount(string nrecText)
    {
        if (string.IsNullOrEmpty(nrecText))
        {
            return 0;
        }

        if (nrecText == "爆")
        {
            return 100;
        }

        if (nrecText.StartsWith("X"))
        {
            return -10;
        }

        if (int.TryParse(nrecText, out int count))
        {
            return count;
        }

        return 0;
    }
}