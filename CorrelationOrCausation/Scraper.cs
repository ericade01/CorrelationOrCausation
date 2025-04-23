using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CorrelationOrCausation;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using static System.Net.WebRequestMethods;
using OpenQA.Selenium.BiDi.Modules.Log;
using SharpDX.DXGI;
using static OpenQA.Selenium.BiDi.Modules.BrowsingContext.ClipRectangle;
using static System.Windows.Forms.Design.AxImporter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.ComponentModel;
using System.Drawing;

namespace CorrelationOrCausation
{
    class Scraper
    {
      

        Api api = new Api();
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_RESTORE = 9;




        public List<string> FinanceUrls = new List<string>
{
    "https://finance.yahoo.com/topic/latest-news/",
    "https://www.reuters.com/finance",


    "https://www.zacks.com",
    "https://www.nasdaq.com/newsroom",
    "https://www.nasdaq.com/newsletters",
    "https://www.nasdaq.com/solutions/fintech/news-and-insights",
    "https://www.nasdaq.com/global-financial-crime-report",
    "https://www.fool.com/investing-news/",
    "https://www.fool.com/market-movers/",
    "https://www.fool.com/industrial-stock-news/",
    "https://www.fool.com/tech-stock-news/",
    "https://www.fool.com/market-trends/",
    "https://www.fool.com/consumer-stock-news/",
    "https://www.fool.com/crypto-news/",
    "https://www.businessinsider.com/markets",
    "https://markets.businessinsider.com/news",
    "https://www.kiplinger.com/investing",
    "https://www.kiplinger.com/economic-forecasts",
    "https://www.valuewalk.com/news/",
    "https://www.foxbusiness.com/markets",
    "https://www.foxbusiness.com/category/money-and-policy",
    "https://www.foxbusiness.com/category/industries",
    "https://www.foxbusiness.com/technology",
    "https://www.investopedia.com/news/",
    "https://www.moneycontrol.com/news/business/markets/",
    "https://www.benzinga.com",
    "https://www.marketbeat.com",
    "https://markets.businessinsider.com",
    "https://www.finviz.com",
    "https://www.stocktwits.com",
    "https://www.tradingview.com",
    "https://www.zerohedge.com",
    "https://www.tipranks.com",
    "https://www.fxstreet.com",
    "https://www.dailyfx.com",
    "https://www.schaeffersresearch.com/content",
    "https://www.moneycontrol.com/stocks/marketinfo/",
    "https://www.barchart.com",
    "https://www.etf.com/news?utm_medium=nav",
    "https://www.federalreserve.gov/monetarypolicy.htm",
    "https://www.tradingeconomics.com",
    "https://www.statista.com",
    "https://www.macrotrends.net",
    "https://www.nyse.com/index",
    "https://www.spglobal.com",
    "https://www.sec.gov/news/pressreleases",
    "https://www.cnbc.com/bonds/",
    "https://www.cnn.com/business/markets",
    "https://www.theguardian.com/business/markets",
    "https://www.oann.com/business/",
    "https://www.investingcube.com",
    "https://www.livecharts.co.uk/",
    "https://www.wallstreetmojo.com",
    "https://www.goodreturns.in",
    "https://www.equitymaster.com",
    "https://www.moneywise.com/investing",
    "https://www.smartasset.com/investing",
    "https://www.talkmarkets.com",
    "https://www.indiainfoline.com",
    "https://www.tradingview.com/markets/",
    "https://www.fxempire.com",
    "https://www.forexfactory.com",
    "https://www.investors.com",
    "https://www.econoday.com",
    "https://www.forexcrunch.com",
    "https://www.calculatedriskblog.com",
    "https://www.realclearmarkets.com",
    "https://www.project-syndicate.org",
    "https://www.nakedcapitalism.com",
    "https://www.alhambrapartners.com",
    "https://www.mises.org",
    "https://www.brookings.edu/topic/markets",
    "https://www.aier.org",
    "https://www.cbo.gov/topics/economic",
    "https://www.crfb.org",
    "https://www.piie.com",
    "https://www.policyuncertainty.com",
    "https://www.asiamarkets.com",
    "https://www.finextra.com",
    "https://www.financemagnates.com",
    "https://www.finbold.com",
    "https://www.pymnts.com/category/economy/",
    "https://www.eubusiness.com/topics/finance",
    "https://www.canadianinvestor.com",
    "https://www.globalfinreg.com",
    "https://www.nasdaqtrader.com",
    "https://www.msci.com/market-classification",
    "https://www.ici.org/statistics",
    "https://www.imf.org/en/News",
    "https://www.worldbank.org/en/news",
    "https://www.oecd.org/newsroom/",
    "https://www.tradingcharts.com",
    "https://www.investingnews.com/markets/stock-market-news/",
    "https://www.bls.gov/eag/eag.us.htm",
    "https://www.statcan.gc.ca/en/subjects-start/economy",
    "https://www.boj.or.jp/en/statistics/outline/",
    "https://www.rba.gov.au/statistics/",
    "https://www.mas.gov.sg/statistics"
};




        public List<string> TwitterUrls = new List<string>
        {
            "https://twitter.com/GoldmanSachs",
            "https://twitter.com/BlackRock",
            "https://twitter.com/MorganStanley",
            "https://twitter.com/CharlesSchwab",
            "https://twitter.com/Fidelity",
            "https://twitter.com/StateStreet",
            "https://twitter.com/Vanguard_Group",
            "https://twitter.com/JPMorgan",
            "https://twitter.com/CNBC",
            "https://twitter.com/BloombergTV",
            "https://twitter.com/YahooFinance",
            "https://twitter.com/ReutersBiz",
            "https://twitter.com/WSJmarkets",
            "https://twitter.com/MarketWatch",
            "https://twitter.com/Investingcom",
            "https://twitter.com/business",
            "https://twitter.com/FinancialTimes",
            "https://twitter.com/ftmarkets",
            "https://twitter.com/zerohedge",
            "https://twitter.com/Benzinga",
            "https://twitter.com/Stocktwits",
            "https://twitter.com/tradingview",
            "https://twitter.com/TheStreet",
            "https://twitter.com/seekingalpha",
            "https://twitter.com/ZacksResearch",
            "https://twitter.com/FinViz_com",
            "https://twitter.com/ETFcom",
            "https://twitter.com/TipRanks",
            "https://twitter.com/macrotrader",
            "https://twitter.com/BreakingMarketNews",
            "https://twitter.com/TheEconomist",
            "https://twitter.com/Financemagnates",
            "https://twitter.com/Moneycontrolcom",
            "https://twitter.com/IndiaFinanceBot",
            "https://twitter.com/IBDinvestors",
            "https://twitter.com/Nasdaq",
            "https://twitter.com/FT",
            "https://twitter.com/RealVision",
            "https://twitter.com/BarronsOnline",
            "https://twitter.com/ValueWalk",
            "https://twitter.com/motleyfool",
            "https://twitter.com/kathylienfx",
            "https://twitter.com/StockMKTNewz",
            "https://twitter.com/stevenmnuchin1",
            "https://twitter.com/elerianm",
            "https://twitter.com/lisaabramowicz1",
            "https://twitter.com/carlquintanilla",
            "https://twitter.com/jimcramer",
            "https://twitter.com/elerianm"
        };

        public bool IsEdgeOpen()
        {
            Process[] processes = Process.GetProcessesByName("msedge");

            foreach (Process proc in processes)
            {
                if (!proc.HasExited && proc.MainWindowHandle != IntPtr.Zero)
                {
                    return true;
                }
            }

            return false;
        }


        public void KillAllMainEdgeProcesses()
        {
            Process[] processes = Process.GetProcessesByName("msedge");

            foreach (Process proc in processes)
            {
                try
                {
                    if (!IsEdgeOpen())
                        return;
                    // Only kill the process if it has no parent (main process),
                    // or it has a main window (not a subprocess)
                    if (!proc.HasExited && proc.MainWindowHandle != IntPtr.Zero)
                    {
                        proc.Kill();
                    }
                }
                catch (Exception)
                {
                    // Swallow exceptions to prevent crash on access denied/system processes
                }
            }
        }


        public void OpenChrome(int index)
        {
            if (IsEdgeOpen())
            {
              //  MessageBox.Show("edge already open");
            }
            else
            {
                if (index >= 0 && index < FinanceUrls.Count)
                {
                    string url = FinanceUrls[index];
                    var existing = Process.GetProcessesByName("msedge");
                    var existingIds = new HashSet<int>();
                    foreach (var p in existing) existingIds.Add(p.Id);

                    var psi = new ProcessStartInfo
                    {
                        FileName = "msedge.exe",
                        Arguments = "--new-window " + url,
                        UseShellExecute = true,


                    };

                    Process.Start(psi);
                    //  MessageBox.Show("1 open");

                }
            }
        }
        public void OpenDeepseek()
        {
            if (IsEdgeOpen())
            {
                MessageBox.Show("edge already open");
            }
            else
            {
               
                    string url = "https://chatgpt.com/";


                    var psi = new ProcessStartInfo
                    {
                        FileName = "msedge.exe",
                        Arguments = "--new-window " + url,
                        UseShellExecute = true,


                    };

                    Process.Start(psi);
                    //  MessageBox.Show("1 open");

                
            }
        }

        public void MoveAllEdgeWindows()
        {
            var edgeProcesses = Process.GetProcessesByName("msedge");

            foreach (var proc in edgeProcesses)
            {
                IntPtr hWnd = proc.MainWindowHandle;
                if (hWnd != IntPtr.Zero)
                {
                    MoveWindow(hWnd, 0, 0, 1200, 800, true);
                }
            }

        }



        public void BringAllEdgeWindowsToFront()
        {
            var edgeProcesses = Process.GetProcessesByName("msedge");

            foreach (var proc in edgeProcesses)
            {
                IntPtr hWnd = proc.MainWindowHandle;
                if (hWnd != IntPtr.Zero)
                {
                    SetForegroundWindow(hWnd);
                    ShowWindow(hWnd, SW_RESTORE);
                }
            }
        }




        public void CollectWebsiteText(int index, int waitTimeSeconds, string outputPath = null)
        {
            if (index < 0 || index >= FinanceUrls.Count) return;
            else { }
                    checkclose:
            if (!IsEdgeOpen())
                OpenChrome(index);
            else
            {
                KillAllMainEdgeProcesses();
                Thread.Sleep(5000); goto checkclose;

            }

            MoveAllEdgeWindows();
            Thread.Sleep(50);
            BringAllEdgeWindowsToFront();
                Thread.Sleep(waitTimeSeconds * 1000);

            
            //44 338
            Cursor.Position = new Point(44, 526);
            Thread.Sleep(50);
            api.MouseLeftDown();
            Thread.Sleep(100);
            api.MouseLeftUp();
            Thread.Sleep(50);
            
         //   api.SendTabKey();
         //   Thread.Sleep(100);


            api.SendCtrlA();
            Thread.Sleep(300);
            api.SendCtrlC();
            Thread.Sleep(500);

            string text = Clipboard.ContainsText() ? Clipboard.GetText() : string.Empty;

          
            if (!string.IsNullOrWhiteSpace(text))
            {
                System.IO.File.AppendAllText(outputPath ?? Path.Combine(Directory.GetCurrentDirectory(), "output.txt"), $"\n=== {FinanceUrls[index]} ===\n{text}\n\n");
              //  MessageBox.Show(text);
               
            }

            KillAllMainEdgeProcesses();
           // MessageBox.Show("Killed");
            Thread.Sleep(600);
        checkclose2:
            if (!IsEdgeOpen())
                return;
            else
            {
                KillAllMainEdgeProcesses();
                Thread.Sleep(1000); goto checkclose2;
            }

        }

        public void CollectMultipleWebsitesText(int numberOfSites, int waitTimeSecondsPerSite, string outputPath = null)
        {
            

            int count = Math.Min(numberOfSites, FinanceUrls.Count);
            for (int i = 0; i < count; i++)
            {
                CollectWebsiteText(i, waitTimeSecondsPerSite, outputPath);
                Thread.Sleep(1000);
            }
        }
        public void cleartextdoc(string outputPath = null)
        {
            System.IO.File.Create(outputPath ?? Path.Combine(Directory.GetCurrentDirectory(), "output.txt")).Close();
        }



        public void SubmitToDeepSeek(string whatToSend)
        {
            // Move cursor and click
            Cursor.Position = new Point(656, 394);
            Thread.Sleep(100);
            api.MouseClick();
            Thread.Sleep(600);

            // Prepare the text to send
            string instructionText = @"
YOU ARE TO EXECUTE THIS EXTRACTION PERFECTLY. DO NOT OMIT, DO NOT INTERPRET, DO NOT SUMMARIZE.

⬇️ CORE INSTRUCTIONS:
1. Extract EVERY article block that contains:
   - A readable, full-length title (any type: news, video, opinion, alert, etc.)
   - A timestamp IF present — if not, mark as ""PENDING""

2. Format EACH ENTRY as:
   ""[FULL_TITLE_IN_ORIGINAL_CAPS]"" ""[TIME_IN_SOURCE_FORMAT or PENDING]""

3. Timestamp is valid if in ANY of these forms:
   - Full date: Apr 19, 2025
   - Relative: 3 hours ago, yesterday, just now
   - If no time found, assign exactly: ""PENDING""

4. STRUCTURE RULES:
   - Time and title must be on same line or within 1 line distance (above or below)
   - If both above and below options exist, use the ABOVE line by default
   - One result per line, in order of appearance
   - Clean format: monospaced font, plain output only

5. CONTENT FILTERING:
   - EXCLUDE: labels like VIDEO, LIVE, ADVERTISEMENT, PROMOTED, READ
   - EXCLUDE: image captions, alt tags, filenames, truncated or partial phrases
   - INCLUDE ONLY: true article titles + their closest time (or PENDING)

⬇️ MANDATORY VERIFICATION LOOP — 5 PASSES:
1. PASS 1: Extract as instructed
2. PASS 2: Reprocess same text — compare to PASS 1. If results differ, restart from scratch
3. PASS 3: Repeat extraction — compare again to prior result. If changed, restart.
4. PASS 4: Repeat — if identical to PASS 3, proceed to final check
5. PASS 5: FINAL integrity sweep — cross-check source to ensure NO additional valid entries were skipped

✴️ IF AT ANY STAGE THE COUNT OR CONTENT OF TITLES DIFFERS — RESET EXTRACTION FROM BEGINNING.

6. DEDUPLICATION RULE:
   - Final output MUST contain unique title strings only
   - Sort in original source order

7. OUTPUT FORMAT MUST BE:
   - Plain block list
   - One title per line
   - Each line formatted as:
     ""[FULL_TITLE_IN_ORIGINAL_CAPS]"" ""[TIME_IN_SOURCE_FORMAT or PENDING]""

🚫 DO NOT return any headers, notes, commentary, summaries, or metadata.

✅ EXAMPLE:
""INFLATION COOLS MORE THAN EXPECTED"" ""Apr 18, 2025""
""TESLA SHARES PLUNGE AFTER EARNINGS"" ""2 hours ago""
""MORGAN STANLEY UPGRADES TECH STOCKS"" ""PENDING""
";



            // Combine instructions with the input text
            string copypaste = instructionText + Environment.NewLine + whatToSend;


            api.SetClipboardText(copypaste);
            Thread.Sleep(10);
            api.SendCtrlV();
            Thread.Sleep(1000);

            api.SendEnterKey();

            //998 609 is "down" key
            
        }



        public List<string> ExtractTitlesWithDates(string rawText)
        {
            List<string> results = new List<string>();
            HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            string datePattern = @"(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s+\d{1,2},\s+\d{4}";
            string blockPattern = $@"
                (?<block>
                    (?:
                        (?<title>[A-Z][^\n]{{20,}})\s*?\n+.*?(?<date>{datePattern})
                    )
                    |
                    (?:
                        (?<date_alt>{datePattern}).*?\n+(?<title_alt>[A-Z][^\n]{{20,}})
                    )
                )";

            Regex regex = new Regex(blockPattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
            MatchCollection matches = regex.Matches(rawText);

            foreach (Match match in matches)
            {
                string title = match.Groups["title"].Success ? match.Groups["title"].Value :
                               match.Groups["title_alt"].Success ? match.Groups["title_alt"].Value : null;
                string date = match.Groups["date"].Success ? match.Groups["date"].Value :
                              match.Groups["date_alt"].Success ? match.Groups["date_alt"].Value : "PENDING";

                if (!string.IsNullOrWhiteSpace(title))
                {
                    title = title.Trim();

                    if (title.Length < 20) continue;
                    if (title.StartsWith("BY ")) continue;
                    if (title.Contains("GETTYIMAGES")) continue;
                    if (title.StartsWith("HTTP") || title.StartsWith("HTTPS")) continue;
                    if (!Regex.IsMatch(title, @"[a-z]")) continue;

                    string formatted = $"\"{title.ToUpper()}\" \"{date}\"";
                    if (!seen.Contains(formatted))
                    {
                        seen.Add(formatted);
                        results.Add(formatted);
                    }
                }
            }

            return results;
        }






    }
}

