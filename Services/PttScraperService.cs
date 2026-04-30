using AutoCatch.Models;
using Microsoft.Playwright;

public class PttScraperService
{
    public async Task<List<SocialPost>> GetPostsFromBoardAsync(
        PttBoardConfig config, CancellationToken ct)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(
            new() { Headless = true }
        );
        var page = await browser.NewPageAsync();

        List<SocialPost> allPosts = [];

        try
        {
            await page.GotoAsync($"https://www.ptt.cc/bbs/{config.Name}/index.html");

            // 處理18歲驗證
            var over18Btn = await page.QuerySelectorAsync("button[name='yes']");
            if (over18Btn != null)
            {
                await over18Btn.ClickAsync();
            }

            while (allPosts.Count < config.NumPost)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                // 等待文章列表載入
                await page.WaitForSelectorAsync(".r-ent");

                // 抓取當前頁面的所有文章節點
                var postNodes = await page.QuerySelectorAllAsync(".r-ent");

                foreach (var node in postNodes)
                {
                    if (ct.IsCancellationRequested)
                    {
                        return allPosts;
                    }

                    var nrecElement = await node.QuerySelectorAsync(".nrec");
                    var nrecText = nrecElement != null ?
                        await nrecElement.InnerTextAsync() : "";
                    int pushCount = ParsePushCount(nrecText);

                    if (pushCount < config.MinNrec)
                    {
                        continue;
                    }

                    var titleElement = await node.QuerySelectorAsync(".title a");
                    if (titleElement == null)
                    {
                        continue;
                    }

                    var title = await titleElement.InnerTextAsync();
                    if (title.Contains("[公告]") || title.Contains("[置底]"))
                    {
                        continue;
                    }

                    if (config.HideGiveMoney &&
                        (title.Contains("發錢") || title.Contains("P幣")))
                    {
                        continue;
                    }

                    if (config.HideReplies &&
                        title.StartsWith("Re:", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var url = await titleElement.GetAttributeAsync("href");
                    if (url == null)
                    {
                        continue;
                    }

                    var authorElement = await node.QuerySelectorAsync(".author");
                    var author = authorElement != null ?
                        await authorElement.InnerTextAsync() : "未知";

                    var dateElement = await node.QuerySelectorAsync(".date");
                    var dateStr = dateElement != null ?
                        await dateElement.InnerTextAsync() : "";

                    allPosts.Add(new SocialPost
                    {
                        Id = url.Split("/").Last().Replace(".html", ""),
                        Content = title,
                        Url = "https://www.ptt.cc" + url,
                        Author = author,
                        Date = ParsePttDate(dateStr),
                        Platform = "PTT",
                        Tags = [config.Name]
                    });

                    if (allPosts.Count >= config.NumPost)
                    {
                        break;
                    }
                }

                if (allPosts.Count < config.NumPost &&
                    !ct.IsCancellationRequested)
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
        }
        catch
        {
            // 異常時仍回傳已抓取的文章
        }

        return allPosts;
    }

    public async Task<List<SocialPost>> CatchAllBoardsAsync(
        List<PttBoardConfig> configs,
        CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var tasks = configs.Where(c => c.Enabled).Select(async config =>
        {
            return await GetPostsFromBoardAsync(
                config, ct
            );
        });

        var results = await Task.WhenAll(tasks);

        return [.. results.SelectMany(x => x).OrderByDescending(p => p.Date)];
    }

    private DateTime ParsePttDate(string dateStr)
    {
        try {
            var now = DateTime.Now;
            var parsed = DateTime.Parse($"{now.Year}/{dateStr.Trim()}");

            // 跨年修正：若解析出的日期比現在晚超過一個月，視為去年
            if (parsed > now.AddMonths(1))
            {
                parsed = parsed.AddYears(-1);
            }

            return parsed;
        } catch {
            return DateTime.Now;
        }
    }

    private int ParsePushCount(string nrecText)
    {
        if (string.IsNullOrWhiteSpace(nrecText))
        {
            return 0;
        }

        var text = nrecText.Trim();

        if (nrecText == "爆")
        {
            return 100;
        }

        if (nrecText.StartsWith("X"))
        {
            return -1;
        }

        if (int.TryParse(nrecText, out int count))
        {
            return count;
        }

        return 0;
    }
}