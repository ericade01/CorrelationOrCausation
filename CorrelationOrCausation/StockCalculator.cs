using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorrelationOrCausation
{
    public class StockCalculator
    {
        public List<StockData> AllData { get; private set; } = new List<StockData>();

        public Dictionary<string, List<StockData>> MonthlyGroups { get; private set; } = new Dictionary<string, List<StockData>>();
        public Dictionary<string, List<StockData>> WeeklyGroups { get; private set; } = new Dictionary<string, List<StockData>>();
        public Dictionary<string, List<StockData>> DailyGroups { get; private set; } = new Dictionary<string, List<StockData>>();
        public Dictionary<string, List<StockData>> HourlyGroups { get; private set; } = new Dictionary<string, List<StockData>>();
        public Dictionary<string, List<StockData>> FiveMinuteGroups { get; private set; } = new Dictionary<string, List<StockData>>();

        public void LoadData(string filePath)
        {
            AllData.Clear();
            MonthlyGroups.Clear();
            WeeklyGroups.Clear();
            DailyGroups.Clear();
            HourlyGroups.Clear();
            FiveMinuteGroups.Clear();

            if (!File.Exists(filePath))
                return;

            var lines = File.ReadAllLines(filePath);

            for (int i = 1; i < lines.Length; i++) // skip header
            {
                var parts = lines[i].Split(',');

                if (DateTime.TryParse(parts[0], out DateTime timestamp) &&
                    double.TryParse(parts[4], out double close)) // Adj Close
                {
                    AllData.Add(new StockData { Timestamp = timestamp, Close = close });
                }
            }

            GroupData();
        }

        private void GroupData()
        {
            MonthlyGroups = AllData
                .GroupBy(x => $"{x.Timestamp.Year}-{x.Timestamp.Month:00}")
                .ToDictionary(g => g.Key, g => g.ToList());

            WeeklyGroups = AllData
                .GroupBy(x => $"W{CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(x.Timestamp, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)}-{x.Timestamp.Year}")
                .ToDictionary(g => g.Key, g => g.ToList());

            DailyGroups = AllData
                .GroupBy(x => x.Timestamp.Date.ToString("yyyy-MM-dd"))
                .ToDictionary(g => g.Key, g => g.ToList());

            HourlyGroups = AllData
                .GroupBy(x => $"{x.Timestamp:yyyy-MM-dd HH}")
                .ToDictionary(g => g.Key, g => g.ToList());

            FiveMinuteGroups = AllData
                .GroupBy(x => $"{x.Timestamp:yyyy-MM-dd HH:mm}")
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public double CalculateAverage(List<StockData> group)
        {
            if (group == null || group.Count == 0) return 0;
            return group.Average(x => x.Close);
        }

        public double CalculateMovingAverage(List<StockData> group)
        {
            if (group == null || group.Count == 0) return 0;
            return group.Average(x => x.Close);
        }

        public double CalculateStandardDeviation(List<StockData> group)
        {
            if (group == null || group.Count == 0) return 0;
            double avg = CalculateAverage(group);
            double sumSquares = group.Sum(x => (x.Close - avg) * (x.Close - avg));
            return Math.Sqrt(sumSquares / group.Count);
        }




    }

    public class runpy
    {
        public List<StockData> RunAndParse(string ticker, string period, string interval)
        {
            string pythonExePath = @"C:\Users\aaron\AppData\Local\Programs\Python\Python311\python.exe";
            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string scriptPath = Path.Combine(exeDir, "ypy.py");

            string arguments = $"\"{scriptPath}\" {ticker} {period} {interval}";

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = pythonExePath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = psi })
            {
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (!string.IsNullOrWhiteSpace(errors) && !errors.StartsWith("Note:"))
                    throw new Exception("Python error: " + errors);

                if (output.StartsWith("ERROR:"))
                    throw new Exception(output);

                return ParseCsvToStockData(output);
            }
        }

        private List<StockData> ParseCsvToStockData(string csv)
        {
            var list = new List<StockData>();
            using (var reader = new StringReader(csv))
            {
                string line;
                bool isFirst = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirst) { isFirst = false; continue; }

                    var parts = line.Split(',');
                    if (parts.Length < 5) continue;

                    if (DateTime.TryParse(parts[0], out DateTime time) &&
                        double.TryParse(parts[4], NumberStyles.Any, CultureInfo.InvariantCulture, out double close))
                    {
                        list.Add(new StockData { Timestamp = time, Close = close });
                    }
                }
            }
            return list;
        }

        public void LoadToCalculator(StockCalculator calc, string ticker, string period, string interval)
        {
            var data = RunAndParse(ticker, period, interval);

            // Clear and refill existing lists — no reassignment
            calc.AllData.Clear();
            calc.AllData.AddRange(data);

            calc.MonthlyGroups.Clear();
            calc.WeeklyGroups.Clear();
            calc.DailyGroups.Clear();
            calc.HourlyGroups.Clear();
            calc.FiveMinuteGroups.Clear();

            // Call internal grouping logic
            var method = typeof(StockCalculator).GetMethod("GroupData", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(calc, null);
        }

    }



}
