using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CorrelationOrCausation
{
    class Stonks
    {
        public async Task<string> GetStockPrice(string symbol)
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync($"http://localhost:8000/price?symbol={symbol}&exchange=NASDAQ");
            var json = JObject.Parse(response);
            return $"Symbol: {json["symbol"]}, Price: {json["price"]}, Summary: {json["summary"]}";
        }


    }
}
