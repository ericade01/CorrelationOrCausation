using System.Text.RegularExpressions;

public static class ScraperYahoo
{
    //https://finance.yahoo.com/topic/latest-news/

    public static List<string> ExtractTitlesFromYahooText(string rawText)
    {
        var lines = rawText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(l => l.Trim())
                           .ToList();

        var results = new List<string>();
        var buffer = new List<string>();
        bool bufferIsAd = false;

        var timeRegex = new Regex(@"\b(\d{1,2} (minutes?|hours?) ago|just now|yesterday|\d{1,2} days ago|[A-Z][a-z]{2} \d{1,2}, \d{4})\b", RegexOptions.IgnoreCase);
        var adRegex = new Regex(@"(?i)\b(adsource|\.ad$|\.Ad$|Ad$|advertisement|promo|sponsored|fisher investments|betterbuck|smartasset|walletjump|motley fool|paradigm press|best-money\.com|online shopping tools)\b");
        var tickerRegex = new Regex(@"^(\^?[A-Z0-9.\-]+|[+-]?\d+(\.\d+)?%)$");

        int start = lines.FindIndex(l => l.Contains("Latest News", StringComparison.OrdinalIgnoreCase));
        int end = lines.FindIndex(start + 1, l => l.StartsWith("Copyright", StringComparison.OrdinalIgnoreCase));
        if (start == -1 || end == -1 || end <= start) return results;

        for (int i = start + 1; i < end - 1; i++)
        {
            string line = lines[i];

            if (adRegex.IsMatch(line))
            {
                buffer.Clear();
                bufferIsAd = false;
                continue; // REPROCESS NEXT LINE as potential start of valid block
            }

            if (line == "•" && timeRegex.IsMatch(lines[i + 1]))
            {
                string time = timeRegex.Match(lines[i + 1]).Value;

                if (!bufferIsAd && buffer.Count > 0)
                {
                    string fullTitle = string.Join(" ", buffer).Trim();
                    if (!string.IsNullOrWhiteSpace(fullTitle))
                    {
                        results.Add($"[{fullTitle}] [{time}]");
                        results.Add("");
                    }
                }

                buffer.Clear();
                bufferIsAd = false;
                i++; // skip time line

                // Skip any ticker garbage
                while (i + 1 < end && tickerRegex.IsMatch(lines[i + 1]))
                {
                    i++;
                }
            }
            else
            {
                buffer.Add(line);
            }
        }

        return results;
    }

}

public static class ScraperReuters
{

    //https://www.reuters.com/business/autos-transportation/
    //https://www.reuters.com/business/aerospace-defense/
    //https://www.reuters.com/business/energy/
    //https://www.reuters.com/business/davos/
    //https://www.reuters.com/business/future-of-money/
    //https://www.reuters.com/business/world-at-work/
    //https://www.reuters.com/business/aerospace-defense/
    //https://www.reuters.com/business/autos-transportation/
    //https://www.reuters.com/business/finance/
    //https://www.reuters.com/business/retail-consumer/
    //https://www.reuters.com/business/future-of-money/
    //https://www.reuters.com/markets/funds/
    //https://www.reuters.com/markets/carbon/
    //https://www.reuters.com/markets/deals/
    //https://www.reuters.com/markets/emerging/
    //https://www.reuters.com/markets/etf/
    //https://www.reuters.com/markets/funds/
    //https://www.reuters.com/markets/wealth/
    //https://www.reuters.com/markets/econ-world/
    //https://www.reuters.com/lifestyle/     // LIKELY EXCLUDE 
    //https://www.reuters.com/science/


    public static List<string> ExtractTitlesFromReutersText(string rawText)
    {
        var lines = rawText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(l => l.Trim())
                           .ToList();

        var results = new List<string>();
        var timeRegex = new Regex(@"\b(\d{1,2}:\d{2} (AM|PM) CDT|\b[A-Z][a-z]{2,8} \d{1,2}, \d{4})\b", RegexOptions.IgnoreCase);
        var adRegex = new Regex(@"(?i)\b(adsource|\.ad$|\.Ad$|Ad$|sponsored|promo|report this ad|fisher investments|betterbuck|smartasset|motley fool|paradigm press|walletjump|best-money\.com|online shopping tools)\b");

        for (int i = 1; i < lines.Count; i++)
        {
            if (timeRegex.IsMatch(lines[i]) && !adRegex.IsMatch(lines[i - 1]) && !adRegex.IsMatch(lines[i]))
            {
                string headline = lines[i - 1];
                string time = timeRegex.Match(lines[i]).Value;

                if (!string.IsNullOrWhiteSpace(headline))
                {
                    results.Add($"[{headline}] [{time}]");
                    results.Add("");
                }
            }
        }

        return results;
    }

    public static List<string> ExtractTitlesFromReutersText2(string rawText)
    {
        var lines = rawText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(l => l.Trim())
                           .ToList();

        var results = new List<string>();

        var timeRegex = new Regex(@"\b(\d{1,2}:\d{2} (AM|PM) CDT|\b[A-Z][a-z]{2,8} \d{1,2}, \d{4})\b", RegexOptions.IgnoreCase);
        var adRegex = new Regex(@"(?i)\b(adsource|\.ad$|\.Ad$|Ad$|sponsored|promo|report this ad|fisher investments|betterbuck|smartasset|motley fool|paradigm press|walletjump|best-money\.com|online shopping tools)\b");
        var badHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Markets Now",
        "More Funds News",
        "Top Video News",
        "More Media & Telecom News",
        "More Business News",
        "Macro Matters"
    };

        for (int i = 1; i < lines.Count; i++)
        {
            if (timeRegex.IsMatch(lines[i]) && !adRegex.IsMatch(lines[i - 1]) && !adRegex.IsMatch(lines[i]))
            {
                string headline = lines[i - 1];
                string time = timeRegex.Match(lines[i]).Value;

                if (!string.IsNullOrWhiteSpace(headline) && !badHeaders.Contains(headline))
                {
                    results.Add($"[{headline}] [{time}]");
                    results.Add("");
                }
            }
        }

        return results;
    }



}


//NASDAQW IS NIGH IMPOSSOIBLE T OWORK WITH
public static class ScraperNasdaq
{
    public static List<string> ExtractTitlesFromNasdaqText(string rawText)
    {
        var lines = rawText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(l => l.Trim())
                           .ToList();

        var results = new List<string>();
        var timeRegex = new Regex(@"^[A-Z][a-z]{2} \d{1,2}, \d{4}$");
        var badTitles = new HashSet<string> { "Latest News" };

        for (int i = 1; i < lines.Count; i++)
        {
            if (timeRegex.IsMatch(lines[i]) &&
                !string.IsNullOrWhiteSpace(lines[i - 1]) &&
                !timeRegex.IsMatch(lines[i - 1]) &&
                !badTitles.Contains(lines[i - 1]))
            {
                results.Add($"[{lines[i - 1]}] [{lines[i]}]");
            }
        }

        return results;
    }


}



