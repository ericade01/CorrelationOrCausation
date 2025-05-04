using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;


namespace CorrelationOrCausation
{
    public partial class Form1 : Form
    {

        //webull 199111


        #region includes
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);
        const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const uint MOUSEEVENTF_RIGHTUP = 0x0010;

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);
        #endregion

        public static bool start = false;
        private runpy runapy = new runpy();
        private Api api = new Api();
        private Scraper scrape = new Scraper();
        private StockCalculator calculator = new StockCalculator();
        public Form1()
        {
            InitializeComponent();



            Thread x = new Thread(new ThreadStart(kk));
            x.IsBackground = true;
            x.Start();
            api.StartListeningForAllAltHotkeys();
        }

        /*   THIS WORKS FOR DEEPSEEK
         * 

            ANALYZE THIS TEXT AND PERFORM THE FOLLOWING WITHOUT FAIL:
            1. Extract EVERY article title regardless of length or formatting
            2. Format EXACTLY as: "[FULL_TITLE_IN_ORIGINAL_CAPS]" "[TIME_IN_SOURCE_FORMAT]"
            3. Include ALL variants (news, videos, opinions) 
            4. NEVER omit any entry that contains both a title and time
            5. If uncertain, PRESERVE the original text rather than filtering
            6. OUTPUT MUST CONTAIN ALL ENTRIES FROM THIS EXAMPLE
            7. USE EASY COPY/PASTE FONT AND ONE ARTICLE PER LINE

            EXAMPLE OUTPUT FORMAT:
            "Sample title text here" "X hours/days ago"
            "Another example title" "yesterday"
            [CONTINUE UNTIL ALL ENTRIES PROCESSED]
        
    
        
         *
         *
                 ***DO NOT THINK. DO NOT INTERPRET. DO NOT SKIP.***

                YOUR TASK IS TO EXTRACT **EVERY** VALID ARTICLE ENTRY FROM THE TEXT BELOW.

                === RULESET (YOU MUST FOLLOW) ===
                1. An article is VALID ONLY IF it has:
                   - A TITLE (headline or story name)
                   - A TIMESTAMP (such as "April 4, 2025", "2:01 PM CDT", "3 hours ago", or "yesterday")

                2. The TITLE must be in ALL CAPS in the final output.
                3. The TIMESTAMP must be EXACTLY as it appears in the source (preserve spacing, symbols, punctuation).

                === FORMATTING INSTRUCTIONS ===
                4. Output format MUST BE:
                   "[FULL_TITLE_IN_ORIGINAL_CAPS]" "[TIME_IN_SOURCE_FORMAT]"

                5. EVERY entry MUST:
                   - Be on its own line
                   - Be wrapped in quotes as shown above
                   - Have NO missing entries
                   - Have NO extra words
                   - Have NO broken formatting

                === SPECIAL INSTRUCTIONS TO NEVER FAIL ===
                6. If a title is followed by a timestamp on the next line — YOU MUST COMBINE THEM.
                7. If a timestamp is on a line that includes a category (e.g. "Marketscategory · April 4, 2025"), extract ONLY the date portion as the timestamp.
                8. If a title is multi-line or broken by line feeds — REASSEMBLE it BEFORE extracting.
                9. If an article is preceded by an image caption or unrelated text — IGNORE those lines and ONLY extract the true title and timestamp.
                10. DO NOT SKIP ANY VALID TITLE + TIME PAIR, EVEN IF THEY ARE SPACED APART OR SEPARATED BY DESCRIPTIONS.

                === OUTPUT VALIDATION (REQUIRED) ===
                11. COUNT ALL OUTPUTS. Your final count **MUST MATCH THE ACTUAL NUMBER** of valid title + timestamp pairs.
                12. If the count is under or over — FIX IT. DO NOT GUESS. DO NOT OMIT.

                === OUTPUT FORMAT EXAMPLE ===
                "ARTICLE TITLE IN FULL CAPS" "April 4, 2025"
                "ANOTHER STORY HERE" "2:01 PM CDT"
                [CONTINUE UNTIL 100% COMPLETE — NO MISTAKES]

                NOW PROCESS THE FOLLOWING TEXT:
                [PASTE RAW TEXT HERE]


         *

         *
         *
         *
         * */






        public void kk()
        {
            bool lastState = false;
            Thread.Sleep(5000);
            while (true)
            {
                label1.Invoke(new System.Windows.Forms.MethodInvoker(delegate { label1.Text = Cursor.Position.ToString(); }));
                if (api.altHotkeys[0] != lastState)
                {
                    lastState = api.altHotkeys[0];

                    if (api.altHotkeys[0])
                    {
                        MessageBox.Show("Hotkey is ON");
                    }
                    else
                    {
                        MessageBox.Show("Hotkey is OFF");
                    }
                }

                Thread.Sleep(50);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {

            scrape.cleartextdoc();
            //  scrape.MoveAllEdgeWindows();
            //  scrape.BringAllEdgeWindowsToFront();

            //  scrape.KillAllMainEdgeProcesses();
            // MessageBox.Show("closeD");
            Thread.Sleep(1000);

            scrape.CollectMultipleWebsitesText(170, 5);

            //pictureBox1.Size = Properties.Resources.test2.Size;
            // pictureBox1.BackgroundImage = api.ConvertToGrayscale(Properties.Resources.test2);

            // var g = api.FindBitmap(api.CaptureAllScreens(), Properties.Resources.test2, 0.02f);

            // MessageBox.Show(g.ToString());
            //   api.MoveCursorTo(g , 5);

            return;


        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {



        }

        private void button2_Click(object sender, EventArgs e)
        {

            var articles = scrape.ExtractTitlesWithDates(tehe());
            Clipboard.SetText(string.Join(Environment.NewLine, articles));




            return;

            scrape.OpenDeepseek();
            Thread.Sleep(1000);
            scrape.BringAllEdgeWindowsToFront();
            scrape.MoveAllEdgeWindows();





            scrape.SubmitToDeepSeek(tehe());
            //599 , 577
            Thread.Sleep(45000);
            Cursor.Position = new Point(731, 641);
            Thread.Sleep(200);
            api.MouseClick();
            Thread.Sleep(200);
            Cursor.Position = new Point(407, 622);
            Thread.Sleep(2200);
            api.MouseClick();
            Thread.Sleep(200);
            return;




        }

        public string tehe()
        {

            string h = "\r\n\r\n=== https://www.fool.com/tech-stock-news/ ===\r\nSkip to main content\r\n\r\nEnable accessibility for low vision\r\n\r\nOpen the accessibility menu\r\nSearch for a company\r\n▲ S&P 500 +155% | ▲ Stock Advisor +796%\r\nAccessibility\r\nLog In\r\nHelp\r\nThe Motley Fool\r\nOur Services\r\n\r\nStock Market News\r\nHow to Invest\r\nRetirement\r\nPersonal Finance\r\nAbout Us\r\n\r\n10 Best Stocks to Buy Now ›\r\nTech Stock News\r\nHow to invest in the hardware, software, services, and components powering technological innovation.\r\n\r\nRecent Articles\r\nAbstract_AI_blocks\r\nApr 12, 2025 by Harsh Chauhan\r\nWhere Will Palantir Technologies Stock Be in 10 Years?\r\nnvidia desktop with nvidia gpu inside and highlighted\r\nApr 12, 2025 by Neil Patel\r\nIs Nvidia a Buy?\r\nGettyImages-906798262\r\nApr 12, 2025 by Adria Cimino\r\nHere's How Artificial Intelligence (AI) Is Driving Profit Growth for These 2 Tech Stocks\r\nGettyImages-1249955556\r\nApr 12, 2025 by Adam Spatacco\r\nCathie Wood Goes Bargain Hunting: 1 \"Magnificent Seven\" Artificial Intelligence (AI) Stock She Just Couldn't Pass Up During the Nasdaq Sell-Off\r\nStocks falling 2022\r\nApr 12, 2025 by Matt Frankel\r\nNasdaq Bear Market: My Top 5 Tech Stocks to Buy Right Now\r\nThe tech sector has been a turbulent place, but it's full of long-term opportunities.\r\nGOOG (3)\r\nApr 12, 2025 by Justin Pope\r\nIs Alphabet a Buy?\r\nMarket volatility and concerns over ChatGPT's popularity have caused the stock to tumble almost 25% from its high.\r\nGettyImages-1347310377\r\nApr 12, 2025 by Rick Orford\r\n3 Quantum Computing Stocks Poised for Explosive Growth\r\nThese quantum computing stocks could explode in value. Don't wait until Wall Street catches on.\r\nASML machine\r\nApr 12, 2025 by Lyle Daly\r\n1 Artificial Intelligence Stock I'm Buying Hand Over First While It's Down 30%\r\ninvesting charts reflected in eyeglasses 1358273775\r\nApr 12, 2025 by Anders Bylund\r\nWant to Invest in Quantum Computing? 3 Stocks That Are Great Buys Right Now\r\nLooking to invest in the future of computing without excessive risk? These three tech powerhouses are building quantum capabilities while generating massive profits from their more well-known core businesses.\r\nApple-Store-fifth-avenue-new-york\r\nApr 12, 2025 by Leo Sun\r\nShould You Forget Apple and Buy These 2 Tech Stocks Instead?\r\nVeriSign and Palo Alto Networks face fewer headwinds than the iPhone maker.\r\nnvidia headquarters outside with black nvidia sign with nvidia logo\r\nApr 12, 2025 by Jose Najarro\r\nIs Google a Threat to Nvidia's AI Dominance?\r\nAlphabet just announced a new Tensor Processing Unit (TPU), Ironwood, focused on AI inference.\r\nGettyImages-172663653\r\nApr 12, 2025 by Timothy Green\r\n2 Reasons to Avoid Amazon Stock as Tariffs Escalate\r\ncybersecurity \r\nApr 12, 2025 by Parkev Tatevosian, CFA\r\nIs Fortinet an Excellent Stock to Buy Amid the Stock Market Chaos?\r\nA person looking at a chart on a mobile phone.\r\nApr 12, 2025 by Matt DiLallo\r\nThink It's Too Late to Buy Verizon? Here's the Biggest Reason Why There's Still Time.\r\nGettyImages-1361008768\r\nApr 12, 2025 by Geoffrey Seiler\r\nStock Market Crash: Is Palantir a Buy?\r\nGettyImages-2078917109\r\nApr 12, 2025 by Geoffrey Seiler\r\n5 Cheap, Leading AI Stocks That Are Screaming Buys in April\r\nMan-specs-computers\r\nApr 12, 2025 by Harsh Chauhan\r\n2 Artificial Intelligence (AI) Stocks to Buy Before They Soar 39% to 77%, According to Wall Street Analysts\r\nInvestor 7\r\nApr 12, 2025 by Trevor Jennewine\r\nWarren Buffett Owns 2 Artificial Intelligence (AI) Stocks That Wall Street Says Could Soar Up to 50%\r\nAI written on circuit board\r\nApr 12, 2025 by Harsh Chauhan\r\nShould You Forget Palantir and Buy This Artificial Intelligence (AI) Stock Instead?\r\nArtificial intelligence 7\r\nApr 12, 2025 by Trevor Jennewine\r\nPrediction: 2 AI Stocks Will Be Worth More Than Palantir Technologies by Early 2026\r\n\r\nLoad More\r\nAd\r\n\r\nAlert: Top Cash Back Card Now Offers 0% Intro APR Until 2026\r\n\r\nEnjoy 0% intro APR on purchases and balance transfers well into 2026—plus up to 5% cash back in popular categories.\r\n\r\nClick Here to Start Earning More ›\r\nMotley Fool Returns\r\n\r\nMotley Fool Stock Advisor\r\nMarket-beating stocks from our flagship service.\r\n\r\nStock Advisor Returns\r\n796%\r\nS&P 500 Returns\r\n155%\r\nCalculated by average return of all stock recommendations since inception of the Stock Advisor service in February of 2002. Returns as of 04/12/2025.\r\n\r\nDiscounted offers are only available to new members. Stock Advisor list price is $199 per year.\r\n\r\nJoin Stock Advisor\r\nCumulative Growth of a $10,000 Investment in Stock Advisor\r\nCalculated by Time-Weighted Return since 2002. Volatility profiles based on trailing-three-year calculations of the standard deviation of service investment returns.\r\n\r\nChart Showing the Cumulative Growth of a $10,000 Investment in Stock Advisor\r\n\r\n\r\nPremium Investing Services\r\nInvest better with The Motley Fool. Get stock recommendations, portfolio guidance, and more from The Motley Fool's premium services.\r\n\r\nView Premium Services\r\nThe Motley Fool\r\nMaking the world smarter, happier, and richer.\r\n\r\nFacebook\r\nTwitter\r\nLinked In\r\nPinterest\r\nYouTube\r\nInstagram\r\nTiktok\r\n© 1995 - 2025 The Motley Fool. All rights reserved.\r\n\r\nMarket data powered by Xignite and Polygon.io.\r\n\r\nAbout The Motley Fool\r\n\r\nAbout Us\r\nCareers\r\nResearch\r\nNewsroom\r\nContact\r\nAdvertise\r\nOur Services\r\n\r\nAll Services\r\nStock Advisor\r\nEpic\r\nEpic Plus\r\nFool Portfolios\r\nFool One\r\nMotley Fool Money\r\nAround the Globe\r\n\r\nFool UK\r\nFool Australia\r\nFool Canada\r\nFree Tools\r\n\r\nCAPS Stock Ratings\r\nDiscussion Boards\r\nCalculators\r\nFinancial Dictionary\r\nAffiliates & Friends\r\n\r\nMotley Fool Asset Management\r\nMotley Fool Wealth Management\r\nMotley Fool Ventures\r\nMotley Fool Foundation\r\nBecome an Affiliate Partner\r\nTerms of Use Privacy Policy Disclosure Policy Accessibility Policy Copyright, Trademark and Patent Information Terms and Conditions Do Not Sell My Personal Information\r\n\r\n\r\n\r\n\r\n=== https://www.fool.com/market-trends/ ===\r\nSkip to main content\r\n\r\nEnable accessibility for low vision\r\n\r\nOpen the accessibility menu\r\nSearch for a company\r\n▲ S&P 500 +155% | ▲ Stock Advisor +796%\r\nAccessibility\r\nLog In\r\nHelp\r\nThe Motley Fool\r\nOur Services\r\n\r\nStock Market News\r\nHow to Invest\r\nRetirement\r\nPersonal Finance\r\nAbout Us\r\n\r\n10 Best Stocks to Buy Now ›\r\nMarket Trends\r\nUse big-picture insights into the broad market to become a better investor.\r\n\r\nRecent Articles\r\nGettyImages-1389866168\r\nApr 12, 2025 by Geoffrey Seiler\r\nHistory Says This Is What Comes Next After a Market Crash and When Stocks Might Recover\r\nGettyImages-1170398354\r\nApr 12, 2025 by Mark Roussin, CPA\r\n3 ETFs to Save Your Portfolio Right Now\r\nThese ETFs bring stability.\r\nGettyImages-2189781864\r\nApr 12, 2025 by Mark Roussin, CPA\r\nStock Market Crash: Do This Now\r\nMarkets are extremely volatile. Do not panic.\r\nWoman with fingers crossed laptop\r\nApr 12, 2025 by Keith Speights\r\nShould You Buy Stocks on the Rebound After the Market Meltdown? Here's What History Shows.\r\n24_10_14 A child in a referee uniform putting their hand up to say stop _MF Dload GettyImages-108219895-1200x800-5b2df79\r\nApr 12, 2025 by Reuben Gregg Brewer\r\nVanguard Financials ETF: Know What You're Buying -- and What You Aren't\r\n21_12_10 Two people looking into a box with their dog _GettyImages-1316497909\r\nApr 12, 2025 by Reuben Gregg Brewer\r\nArtificial Intelligence: Think Outside the AI Box With This Vanguard ETF\r\nStock Chart Crash Correction Plunge Bounce Bear Market Bar Trend Invest Crypto Getty\r\nApr 12, 2025 by Sean Williams\r\nWall Street's Volatility Index (VIX) Has Done This 21 Times in the Last 35 Years -- and It Has a Perfect Track Record of Forecasting Future Stock Moves\r\nUnder rare circumstances, the CBOE Volatility Index has a 100% success rate forecasting directional moves in the S&P 500.\r\nMoney What Next\r\nApr 11, 2025 by Travis Hoium\r\nHow to Invest in Today's Market\r\nIt's time to focus on fundamentals more than ever.\r\nVigilante photo\r\nApr 11, 2025 by Bram Berkowitz\r\nJPMorgan CEO Jamie Dimon Puts the Odds of a Recession at a Coin Flip, but He Says This Economic Cycle Is Different for 1 Reason\r\nBuffett9 TMF\r\nApr 11, 2025 by Adam Spatacco\r\nIf Trump's Tariff Agenda Has You Afraid to Invest Right Now, Keep This Famous Warren Buffett Quote in Mind\r\nTrump's sweeping tariff policies have investors tense and anxious right now.\r\nGettyImages-1964849486\r\nApr 11, 2025 by Mark Roussin, CPA\r\nTime to Load Up on the New Look of SCHD\r\nSCHD is a proven compounder of wealth.\r\nNye7gCNlRlmArIdgJaBGyw\r\nApr 11, 2025 by TMF Breakfast News\r\nBreakfast News: China Hits Back (Again)\r\nImport tariffs raised by China, Alphabet targets agency contracts, Stellantis sees shipment drop, and more...\r\nbear figurine with stock market chart\r\nApr 11, 2025 by Katie Brockman\r\nThis Is the Biggest Investing Mistake You Could Make During a Stock Market Crash\r\nRebuilding a broken piggy bank\r\nApr 11, 2025 by Anders Bylund\r\nS&P 500's Biggest Jump Since 2008: Why Smart Investors Are Looking Beyond the Tariff Rally\r\nMarket whiplash got you worried? Here's why long-term investors should stay calm during tariff-driven volatility (like any other headline-worthy market crash or rally).\r\nA person smiling and sitting at a table in-front of a laptop computer. \r\nApr 11, 2025 by Daniel Foelber\r\n3 Ways to Keep Your Portfolio Safe During Tariff Volatility\r\nConcerned investor looking at a stock chart on a computer.\r\nApr 11, 2025 by David Jagielski\r\nThe Nasdaq Crashed More Than 10% in the First Quarter. That's Just the 4th Time That Has Happened in 25 Years. What Should Investors Do?\r\nGettyImages-1276837766\r\nApr 11, 2025 by Adria Cimino\r\nShould You Really Buy Stocks During Market Turmoil? History Offers an Answer That May Surprise You.\r\nstacks of wooden blocks forming a rising chart\r\nApr 11, 2025 by James Brumley\r\nThe Best Growth ETF to Invest $1,000 in Right Now\r\nThere are several good ones to choose from, but current circumstances make one fund in particular stand out.\r\nMarket 1\r\nApr 11, 2025 by Trevor Jennewine\r\nHere's the Average Stock Market Return in the Last 10 Years and What Wall Street Expects in 2025\r\nperson sitting at a computer looking uncertain\r\nApr 11, 2025 by Katie Brockman\r\nShould You Still Invest During a Stock Market Downturn? It Depends on This One Question.\r\n\r\nLoad More\r\nAd\r\n\r\nAlert: Top Cash Back Card Now Offers 0% Intro APR Until 2026\r\n\r\nEnjoy 0% intro APR on purchases and balance transfers well into 2026—plus up to 5% cash back in popular categories.\r\n\r\nClick Here to Start Earning More ›\r\nStart Your Mornings Smarter!\r\nWake up to the latest market news, company insights, and a bit of Foolish fun—all wrapped up in one quick, easy-to-read email every market day.\r\nEnter Email Address:\r\nEnter Email\r\n \r\nBy submitting your email address, you consent to us keeping you informed about updates to our website and about other products and services that we think might interest you. Please read our Privacy Statement and Terms & Conditions.\r\n\r\n\r\nPremium Investing Services\r\nInvest better with The Motley Fool. Get stock recommendations, portfolio guidance, and more from The Motley Fool's premium services.\r\n\r\nView Premium Services\r\nThe Motley Fool\r\nMaking the world smarter, happier, and richer.\r\n\r\nFacebook\r\nTwitter\r\nLinked In\r\nPinterest\r\nYouTube\r\nInstagram\r\nTiktok\r\n© 1995 - 2025 The Motley Fool. All rights reserved.\r\n\r\nMarket data powered by Xignite and Polygon.io.\r\n\r\nAbout The Motley Fool\r\n\r\nAbout Us\r\nCareers\r\nResearch\r\nNewsroom\r\nContact\r\nAdvertise\r\nOur Services\r\n\r\nAll Services\r\nStock Advisor\r\nEpic\r\nEpic Plus\r\nFool Portfolios\r\nFool One\r\nMotley Fool Money\r\nAround the Globe\r\n\r\nFool UK\r\nFool Australia\r\nFool Canada\r\nFree Tools\r\n\r\nCAPS Stock Ratings\r\nDiscussion Boards\r\nCalculators\r\nFinancial Dictionary\r\nAffiliates & Friends\r\n\r\nMotley Fool Asset Management\r\nMotley Fool Wealth Management\r\nMotley Fool Ventures\r\nMotley Fool Foundation\r\nBecome an Affiliate Partner\r\nTerms of Use Privacy Policy Disclosure Policy Accessibility Policy Copyright, Trademark and Patent Information Terms and Conditions Do Not Sell My Personal Information\r\n\r\n\r\n\r\n\r\n=== https://www.fool.com/consumer-stock-news/ ===\r\nSkip to main content\r\n\r\nEnable accessibility for low vision\r\n\r\nOpen the accessibility menu\r\nSearch for a company\r\n▲ S&P 500 +155% | ▲ Stock Advisor +796%\r\nAccessibility\r\nLog In\r\nHelp\r\nThe Motley Fool\r\nOur Services\r\n\r\nStock Market News\r\nHow to Invest\r\nRetirement\r\nPersonal Finance\r\nAbout Us\r\n\r\nTop 10 Stocks to Buy Now ›\r\nConsumer Stock News\r\nLearn more about the best ways to invest in the brands and products you see every day.\r\n\r\nRecent Articles\r\nretired man finances laptop review\r\nApr 12, 2025 by Parkev Tatevosian, CFA\r\nShould You Buy Tractor Supply Stock Right Now?\r\nstore aisle 5 below dollar general dollar tree-1201x775-6ad09b4\r\nApr 12, 2025 by Jeremy Bowman\r\nThis Recession-Resistant Stock Is Up 16% This Year. Here's Why It Can Beat Trump's Tariffs.\r\nGettyImages-1996136972-1201x896-943533b\r\nApr 12, 2025 by Lawrence Rothman, CFA\r\nBest Stock to Buy Right Now: Walmart vs. Target\r\nCoffee (2)\r\nApr 12, 2025 by Lawrence Nga\r\n3 Reasons Dutch Bros Is the Stock to Watch in 2025\r\nDutch Bros is an up-and-coming growth stock.\r\nAmazon-Tariffs-Thumbnail\r\nApr 12, 2025 by Motley Fool YouTube\r\nIs Amazon Stock Tariff-Proof?\r\nentering ride sharing car\r\nApr 12, 2025 by James Brumley\r\n3 Reasons to Buy Uber Stock Like There's No Tomorrow\r\nThe stock hasn't been a great performer lately, but it's difficult to find a reason why.\r\ndrive-through customer receiving coffee order\r\nApr 12, 2025 by Anders Bylund\r\nWhere Will Dutch Bros Stock Be in 3 Years?\r\nDutch Bros is brewing ambitious expansion plans despite economic challenges. Check out why this rapidly expanding chain might be worth watching despite its premium valuation.\r\nperson in glasses screen reflected\r\nApr 12, 2025 by Parkev Tatevosian, CFA\r\nU.S. Inflation Report Had Good News for Amazon Stock and Unfortunate News for Tesla Stock Investors\r\ninvestor buying selling stock trading app\r\nApr 12, 2025 by John Ballard, Jeremy Bowman, and Jennifer Saibil\r\n3 Brilliant Stocks Down 51% to 77% to Buy Right Now\r\nGettyImages-609087206\r\nApr 12, 2025 by Parkev Tatevosian, CFA\r\nShould You Buy Roblox Stock?\r\nPerson looking at trading charts reflecting in glasses\r\nApr 12, 2025 by Parkev Tatevosian, CFA\r\nShould You Buy Dutch Bros Stock on the Dip?\r\nDefensive Stocks Warren Buffett Owns\r\nApr 12, 2025 by Dan Victor\r\nThe Best Warren Buffett Stocks to Buy With $300 Right Now\r\n22_01_17 Two adults and two children in a room with packing boxes _GettyImages-922730214\r\nApr 12, 2025 by Reuben Gregg Brewer\r\nDoes Opendoor's Business Model Have a Fatal Flaw? 1 Thing Investors Should Watch Before Buying the Stock\r\ndutch_bros_coffee_shop_with_logo_BROS\r\nApr 12, 2025 by Will Healy\r\nIs Dutch Bros Stock a Buy, Sell, or Hold in 2025?\r\nwoman holding a piggy bank\r\nApr 12, 2025 by Leo Sun\r\nThis Growth Stock Could Be the Best Investment of the Decade\r\nMercadoLibre still offers plenty of growth at a reasonable valuation.\r\n25_04_08 Mickey and Minnie Mouse dressed up _MF Dload_Source Disney\r\nApr 12, 2025 by Reuben Gregg Brewer\r\nIs FuboTV: A Buy, Sell, or Hold in 2025?\r\nGettyImages-1062733970\r\nApr 11, 2025 by Keith Noonan\r\nWhy Newsmax Stock Plummeted Today\r\nNewsmax Lawsuit Crash 2025\r\nApr 11, 2025 by Johnny Rice\r\nWhy Newsmax Stock Is Plummeting This Week\r\nGettyCruiseCouples\r\nApr 11, 2025 by Rick Munarriz\r\n3 No-Brainer Cruise Line Stocks to Buy Right Now\r\nThe ocean waters have gotten wavy this year, but there's opportunity in the volatility for these three cruise-related investments.\r\nInvesting in stocks\r\nApr 11, 2025 by Lee Samaha\r\n2 Warren Buffett Stocks to Buy Hand Over Fist in April\r\n\r\nLoad More\r\nAd\r\n\r\nAlert: Top Cash Back Card Now Offers 0% Intro APR Until 2026\r\n\r\nEnjoy 0% intro APR on purchases and balance transfers well into 2026—plus up to 5% cash back in popular categories.\r\n\r\nClick Here to Start Earning More ›\r\nMotley Fool Returns\r\n\r\nMotley Fool Stock Advisor\r\nMarket-beating stocks from our flagship service.\r\n\r\nStock Advisor Returns\r\n796%\r\nS&P 500 Returns\r\n155%\r\nCalculated by average return of all stock recommendations since inception of the Stock Advisor service in February of 2002. Returns as of 04/12/2025.\r\n\r\nDiscounted offers are only available to new members. Stock Advisor list price is $199 per year.\r\n\r\nJoin Stock Advisor\r\nCumulative Growth of a $10,000 Investment in Stock Advisor\r\nCalculated by Time-Weighted Return since 2002. Volatility profiles based on trailing-three-year calculations of the standard deviation of service investment returns.\r\n\r\nChart Showing the Cumulative Growth of a $10,000 Investment in Stock Advisor\r\n\r\n\r\nPremium Investing Services\r\nInvest better with The Motley Fool. Get stock recommendations, portfolio guidance, and more from The Motley Fool's premium services.\r\n\r\nView Premium Services\r\nThe Motley Fool\r\nMaking the world smarter, happier, and richer.\r\n\r\nFacebook\r\nTwitter\r\nLinked In\r\nPinterest\r\nYouTube\r\nInstagram\r\nTiktok\r\n© 1995 - 2025 The Motley Fool. All rights reserved.\r\n\r\nMarket data powered by Xignite and Polygon.io.\r\n\r\nAbout The Motley Fool\r\n\r\nAbout Us\r\nCareers\r\nResearch\r\nNewsroom\r\nContact\r\nAdvertise\r\nOur Services\r\n\r\nAll Services\r\nStock Advisor\r\nEpic\r\nEpic Plus\r\nFool Portfolios\r\nFool One\r\nMotley Fool Money\r\nAround the Globe\r\n\r\nFool UK\r\nFool Australia\r\nFool Canada\r\nFree Tools\r\n\r\nCAPS Stock Ratings\r\nDiscussion Boards\r\nCalculators\r\nFinancial Dictionary\r\nAffiliates & Friends\r\n\r\nMotley Fool Asset Management\r\nMotley Fool Wealth Management\r\nMotley Fool Ventures\r\nMotley Fool Foundation\r\nBecome an Affiliate Partner\r\nTerms of Use Privacy Policy Disclosure Policy Accessibility Policy Copyright, Trademark and Patent Information Terms and Conditions Do Not Sell My Personal Information\r\n\r\n\r\n\r\n\r\n=== https://www.fool.com/crypto-news/ ===\r\nSkip to main content\r\n\r\nEnable accessibility for low vision\r\n\r\nOpen the accessibility menu\r\nSearch for a company\r\n▲ S&P 500 +155% | ▲ Stock Advisor +796%\r\nAccessibility\r\nLog In\r\nHelp\r\nThe Motley Fool\r\nOur Services\r\n\r\nStock Market News\r\nHow to Invest\r\nRetirement\r\nPersonal Finance\r\nAbout Us\r\n\r\nTop 10 Stocks to Buy Now ›\r\nCrypto News\r\nRead our latest insights into cryptocurrencies, companies in the industry, and related topics.\r\n\r\nRecent Articles\r\nGetty Images gold coin with bitcoin symbol on it -- cryptocurrency BTC-2128x1409-3b482f4\r\nApr 12, 2025 by Adam Levy\r\nPresident Donald Trump Just Instituted 2 Key Policies That Could Trigger Bitcoin's Next Bull Run\r\nTrump's policies should push more institutional investors to adopt Bitcoin.\r\nGettyImages-1214175996-1200x800-5b2df79\r\nApr 12, 2025 by Ryan Vanzo\r\nEvery BTC (Bitcoin) Investor Should Keep an Eye on This Number in 2025\r\nAnalyst looking at charts.\r\nApr 12, 2025 by David Jagielski\r\nCould Shiba Inu More Than Triple in Value and Catch Up to Dogecoin?\r\ndistressed investor considers papers at table\r\nApr 12, 2025 by Alex Carchidi\r\nWith Tariffs on the Menu, Is XRP Worth Buying Right Now?\r\ncryptocurrency tracking on phone\r\nApr 11, 2025 by Travis Hoium\r\nWhy Bitcoin, Ethereum, and Dogecoin Rallied on Friday\r\nviewing charts on printouts and computer screens 1332377628\r\nApr 11, 2025 by Ryan Vanzo\r\nEvery Dogecoin (DOGE) Investor Should Keep an Eye on This Number in 2025\r\ninvestor thinks while looking out a window\r\nApr 11, 2025 by Alex Carchidi\r\nThis $293 Million Asset Shows Why XRP's Future Is Bright\r\nPeople Talking\r\nApr 11, 2025 by Bram Berkowitz\r\nRipple Just Made a Splash With a Huge $1.25 Billion Acquisition. Will It Be a Game Changer for XRP?\r\nGettyImages-1340545324-5556x3778-e16a9c0\r\nApr 11, 2025 by Emma Newbery\r\nMight Bitcoin Be Tariff-Proof?\r\nBitcoin isn't yet the safe asset people are looking for.\r\ntwo investors put sticky notes on glass wall\r\nApr 11, 2025 by Alex Carchidi\r\nIs XRP a Buy if There's a Bear Market?\r\nXRP 2025 Trump\r\nApr 10, 2025 by Johnny Rice\r\nWhy XRP (Ripple) Is Plummeting Today\r\nHappy older couple with laptop\r\nApr 10, 2025 by Dominic Basulto\r\n1 Unstoppable Investment Strategy for Buying Bitcoin During a Market Decline\r\nGolden bitcoin coin representing the electronic currency\r\nApr 10, 2025 by Dominic Basulto\r\nIf Online Prediction Markets Are Any Guide, Bitcoin Still Has a 35% Chance of Hitting $125,000 in 2025\r\ninvestor ponders a screen\r\nApr 10, 2025 by Alex Carchidi\r\nIs Solana Still Worth Buying in a Bear Market?\r\nBitcoin symbol on red question mark\r\nApr 10, 2025 by Anders Bylund\r\nShould You Buy Bitcoin While It's Under $85,000?\r\nBitcoin's price has fallen 25% from a recent all-time high. Is this a buying opportunity or the start of another crypto winter?\r\nstylus on screen with charts 1402808568\r\nApr 10, 2025 by Ryan Vanzo\r\nEvery XRP (Ripple) Investor Should Keep an Eye on These 2 Numbers in 2025\r\nBitcoin Digital\r\nApr 10, 2025 by Travis Hoium\r\nCrypto Is in a Tailspin, but It's Time to Buy Coinbase\r\nCoinbase stock is cheap, and the business won't be crushed if crypto trading falls.\r\npensive investor considers computer\r\nApr 10, 2025 by Alex Carchidi\r\nIs Ethereum a Dirt Cheap Bear Market Buy?\r\nAn investor curiously looking at charts on a dual monitor computer\r\nApr 10, 2025 by Anthony Di Pizio\r\nXRP (Ripple) Crashed Below $2. Buy the Dip, or Run for the Hills?\r\nGettyImages-1366942177\r\nApr 9, 2025 by Keith Noonan\r\nWhy XRP Is Soaring Today\r\n\r\nLoad More\r\nAd\r\n\r\nLimited Time: Get a $250 Bonus with This Cash Back Card!\r\n\r\nEarn $250 when you spend $500 in 3 months—that's an easy 50% return! Plus, enjoy 0% intro APR for 15 months & up to 5% cash back.\r\n\r\nClaim this limited time offer now ›\r\nStart Your Mornings Smarter!\r\nWake up to the latest market news, company insights, and a bit of Foolish fun—all wrapped up in one quick, easy-to-read email every market day.\r\nEnter Email Address:\r\nEnter Email\r\n \r\nBy submitting your email address, you consent to us keeping you informed about updates to our website and about other products and services that we think might interest you. Please read our Privacy Statement and Terms & Conditions.\r\n\r\n\r\nPremium Investing Services\r\nInvest better with The Motley Fool. Get stock recommendations, portfolio guidance, and more from The Motley Fool's premium services.\r\n\r\nView Premium Services\r\nThe Motley Fool\r\nMaking the world smarter, happier, and richer.\r\n\r\nFacebook\r\nTwitter\r\nLinked In\r\nPinterest\r\nYouTube\r\nInstagram\r\nTiktok\r\n© 1995 - 2025 The Motley Fool. All rights reserved.\r\n\r\nMarket data powered by Xignite and Polygon.io.\r\n\r\nAbout The Motley Fool\r\n\r\nAbout Us\r\nCareers\r\nResearch\r\nNewsroom\r\nContact\r\nAdvertise\r\nOur Services\r\n\r\nAll Services\r\nStock Advisor\r\nEpic\r\nEpic Plus\r\nFool Portfolios\r\nFool One\r\nMotley Fool Money\r\nAround the Globe\r\n\r\nFool UK\r\nFool Australia\r\nFool Canada\r\nFree Tools\r\n\r\nCAPS Stock Ratings\r\nDiscussion Boards\r\nCalculators\r\nFinancial Dictionary\r\nAffiliates & Friends\r\n\r\nMotley Fool Asset Management\r\nMotley Fool Wealth Management\r\nMotley Fool Ventures\r\nMotley Fool Foundation\r\nBecome an Affiliate Partner\r\nTerms of Use Privacy Policy Disclosure Policy Accessibility Policy Copyright, Trademark and Patent Information Terms and Conditions Do Not Sell My Personal Information\r\n\r\n\r\n\r\n ";


            return h;

        }


        Stonks stonk = new Stonks();

        private async void button3_Click(object sender, EventArgs e)
        {
            /*
            string symbol = "NVDA"; // or pull from textbox
            string result = await stonk.GetStockPrice(symbol);



            label2.Text = result;


            return;

            */
         //   Thread.Sleep(1000);
       //     api.ScrollDown(5000, 3);

         //   return;

            var growthResults = TradingCalculator.CalculateGrowth(10000m, 7, 0.002m, 360);
            textBox3.Text = string.Join(Environment.NewLine, growthResults.Select((value, index) => $"Day {index + 1}: {value:C2}"));



            return;


            var raw = Clipboard.GetText();
            var articles = ScraperNasdaq.ExtractTitlesFromNasdaqText(raw);

            if (articles.Count > 0)
            {
                Clipboard.SetText(string.Join("\n", articles));
                label2.Text = $"Extracted {articles.Count / 2} articles."; // because of blank lines
            }
            else
            {
                label2.Text = "No articles found.";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string filePath = @"C:\Users\aaron\Desktop\stonk\1\CorrelationOrCausation\CorrelationOrCausation\bin\Debug\net8.0-windows7.0\yahoo finance\nvda_5m.csv";


            calculator.LoadData(filePath);

            //DisplayAllDataFlexible(calculator.AllData);

            //  DisplayAllDataFlexible(calculator.MonthlyGroups["2025-03"], "2025-03");
            //DisplayAllDataFlexible(calculator.DailyGroups["2025-04-01"], "2025-04-01");
            //DisplayAllDataFlexible(calculator.FiveMinuteGroups["2025-04-01 09:35"], "2025-04-01 09:35");





            // Now you have:
            // calculator.AllData         -> all data
            // calculator.MonthlyGroups   -> grouped per month
            // calculator.WeeklyGroups    -> grouped per week
            // calculator.DailyGroups     -> grouped per day
            // calculator.HourlyGroups    -> grouped per hour
            // calculator.FiveMinuteGroups-> grouped per 5-minute

            // Example: you could now compute something when ready:
            double totalAverage = calculator.CalculateAverage(calculator.MonthlyGroups["2025-04"]);

             totalAverage = calculator.CalculateAverage(calculator.DailyGroups["2025-04-01"]);
            MessageBox.Show(totalAverage.ToString());
            totalAverage = calculator.CalculateAverage(calculator.DailyGroups["2025-04-02"]);
            MessageBox.Show(totalAverage.ToString());

            //

        }


        private void DisplayAllDataFlexible(IEnumerable<StockData> data, string groupName = null)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(groupName))
            {
                sb.AppendLine($"Group: {groupName}");
            }

            foreach (var item in data)
            {
                sb.AppendLine($"{item.Timestamp:yyyy-MM-dd HH:mm}  ->  {item.Close}");
            }

            textBox3.Text = sb.ToString();
        }



        private void DisplayMultipleGroups(Dictionary<string, List<StockData>> groups, List<string> selectedGroupKeys)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var key in selectedGroupKeys)
            {
                if (groups.TryGetValue(key, out var dataList))
                {
                    sb.AppendLine($"Group: {key}");
                    foreach (var item in dataList)
                    {
                        sb.AppendLine($"{item.Timestamp:yyyy-MM-dd HH:mm}  ->  {item.Close}");
                    }
                    sb.AppendLine(); // extra spacing
                }
            }

            textBox3.Text = sb.ToString();
        }









    }


    public class TradingCalculator
    {
       public static List<decimal> CalculateGrowth(decimal deposit, int tradesPerDay, decimal ratePerTrade, int totalDays)
        {
            List<decimal> balances = new List<decimal>();
            decimal balance = deposit;

            for (int day = 1; day <= totalDays; day++)
            {
                for (int trade = 1; trade <= tradesPerDay; trade++)
                {
                    balance *= (1 + ratePerTrade);
                }

                balances.Add(balance);
            }

            return balances;
        }








    }









}
