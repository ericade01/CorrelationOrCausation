using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using System.Linq;
using HtmlAgilityPack;
using System.Net.Http;

public class Scraper2
{
    // Supported domains (example input URLs):
    // - Yahoo Finance: https://finance.yahoo.com/news/...
    // - Reuters: https://www.reuters.com/...
    // - The Motley Fool: https://www.fool.com/...
    // - MarketWatch: https://www.marketwatch.com/latest-news
    // - CNBC: https://www.cnbc.com/latest/
    // - Investing.com: https://www.investing.com/news/

    // Base function: Extracts formatted article titles and timestamps from raw HTML and source URL
    public List<string> ExtractFormattedArticles(string html, string sourceUrl)
    {
        if (string.IsNullOrEmpty(html) || string.IsNullOrEmpty(sourceUrl))
            return null;
        string host;
        try
        {
            Uri uri = new Uri(sourceUrl);
            host = uri.Host.ToLower();
        }
        catch
        {
            // Invalid URL format
            return null;
        }
        List<string> results = null;
        if (host.Contains("finance.yahoo"))
        {
            results = ParseYahooFinance(html);
        }
        else if (host.Contains("reuters.com"))
        {
            results = ParseReuters(html);
        }
        else if (host.Contains("fool.com"))
        {
            results = ParseMotleyFool(html);
        }
        else if (host.Contains("marketwatch.com"))
        {
            results = ParseMarketWatch(html);
        }
        else if (host.Contains("cnbc.com"))
        {
            results = ParseCNBC(html);
        }
        else if (host.Contains("investing.com"))
        {
            results = ParseInvesting(html);
        }
        else
        {
            // Domain not supported
            return null;
        }
        // Normalize output: trim, collapse whitespace, uppercase
        if (results != null)
        {
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i] == null) continue;
                string entry = results[i].Trim();
                entry = Regex.Replace(entry, @"\s+", " ");
                results[i] = entry.ToUpper();
            }
        }
        return results;
    }

    // Fetch HTML with HttpClient (using spoofed User-Agent) and parse articles
    public List<string> GetArticlesFromUrl(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            // Spoof headers to mimic a browser
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            try
            {
                string html = client.GetStringAsync(url).Result;
                return ExtractFormattedArticles(html, url);
            }
            catch
            {
                return null;
            }
        }
    }

    // Parsing for Yahoo Finance
    public List<string> ParseYahooFinance(string html)
    {
        var doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(html);
        var result = new List<string>();
        // Yahoo Finance articles are listed in <li class="js-stream-content">
        var items = doc.DocumentNode.SelectNodes("//li[contains(@class, 'js-stream-content')]");
        if (items == null) return null;
        foreach (var li in items)
        {
            var anchor = li.SelectSingleNode(".//a[contains(@href, '/news/')]");
            if (anchor == null) continue;
            string title = WebUtility.HtmlDecode(anchor.InnerText ?? "").Trim();
            if (string.IsNullOrEmpty(title)) continue;
            // Find timestamp if available (e.g. "2 hours ago" or "Apr 20, 2025")
            string timeText = null;
            var timeNode = li.SelectSingleNode(".//*[contains(text(),'ago') or contains(text(),'AM') or contains(text(),'PM')]");
            if (timeNode != null)
            {
                timeText = WebUtility.HtmlDecode(timeNode.InnerText).Trim();
                // If source and time are combined (e.g. "Reuters • 2 hours ago"), extract time part after "•"
                if (timeText.Contains("•"))
                {
                    int idx = timeText.LastIndexOf('•');
                    if (idx != -1)
                        timeText = timeText.Substring(idx + 1).Trim();
                }
            }
            string output = title;
            if (!string.IsNullOrEmpty(timeText))
            {
                output += " " + timeText;
            }
            result.Add(output);
        }
        return result.Count > 0 ? result : null;
    }

    // Parsing for Reuters
    public List<string> ParseReuters(string html)
    {
        var doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(html);
        var result = new List<string>();
        // Try older Reuters format (archive pages with story-title class)
        var titleNodes = doc.DocumentNode.SelectNodes("//h3[contains(@class, 'story-title')]");
        if (titleNodes != null)
        {
            foreach (var titleNode in titleNodes)
            {
                string title = WebUtility.HtmlDecode(titleNode.InnerText ?? "").Trim();
                if (string.IsNullOrEmpty(title)) continue;
                string timeText = null;
                var timeNode = titleNode.SelectSingleNode(".//following-sibling::span[contains(@class, 'timestamp')]");
                if (timeNode == null)
                {
                    var parent = titleNode.ParentNode;
                    if (parent != null)
                        timeNode = parent.SelectSingleNode(".//span[contains(@class, 'timestamp')]");
                }
                if (timeNode != null)
                {
                    timeText = WebUtility.HtmlDecode(timeNode.InnerText).Trim();
                }
                string output = title;
                if (!string.IsNullOrEmpty(timeText))
                {
                    output += " " + timeText;
                }
                result.Add(output);
            }
            if (result.Count > 0) return result;
        }
        // Try newer Reuters format (articles in <article> tags)
        var articles = doc.DocumentNode.SelectNodes("//article");
        if (articles != null)
        {
            foreach (var art in articles)
            {
                string title = null;
                string timeText = null;
                var titleNode = art.SelectSingleNode(".//h2") ?? art.SelectSingleNode(".//h3") ?? art.SelectSingleNode(".//h1");
                if (titleNode != null)
                {
                    var a = titleNode.SelectSingleNode(".//a");
                    title = WebUtility.HtmlDecode((a != null ? a.InnerText : titleNode.InnerText) ?? "").Trim();
                }
                var timeNode = art.SelectSingleNode(".//time") ?? art.SelectSingleNode(".//*[contains(text(),'ago')]");
                if (timeNode != null)
                {
                    timeText = WebUtility.HtmlDecode(timeNode.InnerText).Trim();
                }
                if (!string.IsNullOrEmpty(title))
                {
                    string output = title;
                    if (!string.IsNullOrEmpty(timeText))
                    {
                        output += " " + timeText;
                    }
                    result.Add(output);
                }
            }
        }
        return result.Count > 0 ? result : null;
    }

    // Parsing for The Motley Fool
    public List<string> ParseMotleyFool(string html)
    {
        var doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(html);
        var result = new List<string>();
        // Identify potential article links (often under /investing/ or related sections)
        var anchors = doc.DocumentNode.SelectNodes("//a");
        if (anchors == null) return null;
        var skipPhrases = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "latest stock picks", "stock advisor", "fool.com"
        };
        foreach (var a in anchors)
        {
            string href = a.GetAttributeValue("href", "").ToLower();
            string text = WebUtility.HtmlDecode(a.InnerText ?? "").Trim();
            if (string.IsNullOrEmpty(text)) continue;
            // Filter for likely article URLs (within investing/retirement sections or similar)
            if (!(href.Contains("/investing/") || href.Contains("/retirement/") || href.Contains("/articles/") || href.Contains("/blogs/")))
                continue;
            if (skipPhrases.Any(p => text.ToLower().Contains(p))) continue;
            if (text.Split(' ').Length < 3) continue;
            // Avoid duplicate titles
            if (result.Any(r => r.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)) continue;
            // Find time (if present) near this anchor
            string timeText = null;
            var parent = a.ParentNode;
            if (parent != null)
            {
                var timeNode = parent.SelectSingleNode(".//time");
                if (timeNode != null)
                {
                    timeText = WebUtility.HtmlDecode(timeNode.InnerText).Trim();
                }
                else
                {
                    string parentText = WebUtility.HtmlDecode(parent.InnerText ?? "");
                    if (parentText.Length > text.Length)
                    {
                        int idx = parentText.IndexOf(text) + text.Length;
                        if (idx >= 0 && idx < parentText.Length)
                        {
                            string afterTitle = parentText.Substring(idx);
                            var match = Regex.Match(afterTitle, @"(\d{1,2}:\d{2}\s?(AM|PM)\s?ET|\d+\s?(hour|minute|day)s?\s?ago|\b(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\b.*\d{4})", RegexOptions.IgnoreCase);
                            if (match.Success)
                            {
                                timeText = match.Value.Trim();
                            }
                        }
                    }
                }
            }
            string output = text;
            if (!string.IsNullOrEmpty(timeText))
            {
                output += " " + timeText;
            }
            result.Add(output);
        }
        return result.Count > 0 ? result : null;
    }

    // Parsing for MarketWatch
    public List<string> ParseMarketWatch(string html)
    {
        var doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(html);
        var result = new List<string>();
        // MarketWatch articles in <div class="article__content">
        var contentBlocks = doc.DocumentNode.SelectNodes("//div[contains(@class, 'article__content')]");
        if (contentBlocks == null) return null;
        foreach (var block in contentBlocks)
        {
            var titleNode = block.SelectSingleNode(".//h3[contains(@class, 'article__headline')]/a");
            if (titleNode == null) continue;
            string title = WebUtility.HtmlDecode(titleNode.InnerText ?? "").Trim();
            if (string.IsNullOrEmpty(title)) continue;
            string timeText = null;
            var timeNode = block.SelectSingleNode(".//span[contains(@class, 'article__timestamp')]");
            if (timeNode != null)
            {
                timeText = WebUtility.HtmlDecode(timeNode.InnerText ?? "").Trim();
            }
            string output = title;
            if (!string.IsNullOrEmpty(timeText))
            {
                output += " " + timeText;
            }
            result.Add(output);
        }
        return result.Count > 0 ? result : null;
    }

    // Parsing for CNBC
    public List<string> ParseCNBC(string html)
    {
        var doc = new HtmlAgilityPack.  HtmlDocument();
        doc.LoadHtml(html);
        var result = new List<string>();
        // CNBC latest news typically list items with time text and link
        var items = doc.DocumentNode.SelectNodes("//li[.//a and (contains(., 'hour ago') or contains(., 'min ago') or contains(., 'Hour Ago') or contains(., 'Min Ago'))]");
        if (items == null) return null;
        foreach (var li in items)
        {
            var anchor = li.SelectSingleNode(".//a");
            if (anchor == null) continue;
            string title = WebUtility.HtmlDecode(anchor.InnerText ?? "").Trim();
            if (string.IsNullOrEmpty(title)) continue;
            if (title.Split(' ').Length < 3) continue;
            string timeText = null;
            var timeNode = li.SelectSingleNode(".//*[contains(text(),'hour') or contains(text(),'Hour') or contains(text(),'min') or contains(text(),'Min')]");
            if (timeNode != null)
            {
                timeText = WebUtility.HtmlDecode(timeNode.InnerText).Trim();
                // Remove author if present (e.g. "By Name 2 hours ago")
                if (timeText.StartsWith("By ", StringComparison.OrdinalIgnoreCase))
                {
                    Match numMatch = Regex.Match(timeText, @"\d");
                    if (numMatch.Success)
                    {
                        timeText = timeText.Substring(numMatch.Index).Trim();
                    }
                }
            }
            else
            {
                string liText = WebUtility.HtmlDecode(li.InnerText ?? "");
                var match = Regex.Match(liText, @"\d+\s+(?:hour|min|day)s?\s+ago", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    timeText = match.Value.Trim();
                }
            }
            string output = title;
            if (!string.IsNullOrEmpty(timeText))
            {
                output += " " + timeText;
            }
            result.Add(output);
        }
        return result.Count > 0 ? result : null;
    }

    // Parsing for Investing.com
    public List<string> ParseInvesting(string html)
    {
        var doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(html);
        var result = new List<string>();
        // Investing.com news articles have "/news/" in URL and include "ago" timestamps
        var anchors = doc.DocumentNode.SelectNodes("//a[contains(@href, '/news/')]");
        if (anchors == null) return null;
        var skipTexts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "breaking news", "currencies", "commodities", "stock markets",
            "economic indicators", "economy", "cryptocurrency", "insider trading",
            "company news", "earnings", "investment ideas", "swot analysis",
            "analyst ratings", "transcripts", "get 100% ad-free experience", "see list"
        };
        var seenTitles = new HashSet<string>();
        foreach (var a in anchors)
        {
            string href = a.GetAttributeValue("href", "");
            string text = WebUtility.HtmlDecode(a.InnerText ?? "").Trim();
            if (string.IsNullOrEmpty(text)) continue;
            if (skipTexts.Contains(text.ToLower())) continue;
            if (Regex.IsMatch(text, @"^[0-9]+$")) continue; // skip pure numeric (comment counts)
            if (text.Split(' ').Length < 3) continue;
            if (seenTitles.Contains(text.ToLower())) continue;
            seenTitles.Add(text.ToLower());
            string timeText = null;
            var li = a.ParentNode.Name == "li" ? a.ParentNode : a.Ancestors("li").FirstOrDefault();
            if (li != null)
            {
                var timeNode = li.SelectSingleNode(".//*[contains(text(),'ago')]");
                if (timeNode != null)
                {
                    timeText = WebUtility.HtmlDecode(timeNode.InnerText).Trim();
                    // Remove source prefix if present (e.g. "By ...•")
                    if (timeText.StartsWith("By ", StringComparison.OrdinalIgnoreCase))
                    {
                        Match numMatch = Regex.Match(timeText, @"\d");
                        if (numMatch.Success)
                        {
                            timeText = timeText.Substring(numMatch.Index).Trim();
                        }
                    }
                }
                else
                {
                    string liText = WebUtility.HtmlDecode(li.InnerText ?? "");
                    var match = Regex.Match(liText, @"\d+\s+(?:hour|minute|day)s?\s+ago", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        timeText = match.Value.Trim();
                    }
                }
            }
            string output = text;
            if (!string.IsNullOrEmpty(timeText))
            {
                output += " " + timeText;
            }
            result.Add(output);
        }
        return result.Count > 0 ? result : null;
    }


    
    public List<string> ExtractYahooTitlesFromRawHtml(string rawHtml)
    {
        var results = new List<string>();
        var lines = rawHtml.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("<h3") && lines[i].Contains("</h3>"))
            {
                string titleHtml = Regex.Match(lines[i], @"<h3[^>]*>(.*?)<\/h3>", RegexOptions.Singleline).Groups[1].Value;
                string title = StripHtml(titleHtml).Trim();
                if (string.IsNullOrWhiteSpace(title)) continue;

                string timestamp = null;
                for (int j = i; j < Math.Min(i + 10, lines.Length); j++)
                {
                    if (lines[j].Contains("class=\"publishing"))
                    {
                        var timeMatch = Regex.Match(lines[j], @"•\s*(.*?)(<\/div>|$)");
                        if (timeMatch.Success)
                        {
                            timestamp = StripHtml(timeMatch.Groups[1].Value).Trim();
                            break;
                        }
                    }
                }

                string formatted = !string.IsNullOrWhiteSpace(timestamp)
                    ? $"\"{title.ToUpper()}\" \"{timestamp}\""
                    : $"\"{title.ToUpper()}\"";

                results.Add(formatted);
            }
        }

        return results;
    }

    private string StripHtml(string input)
    {
        return Regex.Replace(WebUtility.HtmlDecode(input), "<.*?>", "").Trim();
    }





}
