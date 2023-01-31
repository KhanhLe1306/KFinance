using KFinance.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace KFinance.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class FmpController : Controller
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = new Uri("https://financialmodelingprep.com/api/v3"),
        };
        private readonly IConfiguration _config;
        public FmpController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet()]
        public async Task<string> GetPrice(RequestDTO requestDTO)
        {
            try
            {
                decimal invested = 0M;
                decimal totalCurrentValue = 0M;
                decimal shares = 0M;
                string apikey = this._config["FMP:Apikey"]!;
                string parameters = $"?apikey={apikey}";

                string url = client.BaseAddress!.ToString() + $"/historical-price-full/{requestDTO.TickerSymbol}";
                client.BaseAddress = new Uri(url);

                using var temp = await client.GetAsync(parameters);
                var a = await temp.Content.ReadAsStringAsync();

                var res = await temp.Content.ReadFromJsonAsync<ResponseFromAPI>();
                string result = string.Empty;
                if (res is not null)
                {
                    var filteredHistory = res.Historical.ToList().Where(h => DateTime.Compare(h.Date, requestDTO.Start) >= 0 && DateTime.Compare(h.Date, requestDTO.End) <= 0).ToList();
                    filteredHistory.Sort();
                    int iterator = 0;
                    int buyCount = 0;
                    while (true)
                    {
                        if (iterator % requestDTO.Frequency == 0)
                        {
                            DateTime date = requestDTO.Start.AddDays(iterator);
                            if (DateTime.Compare(date, filteredHistory[^1].Date) > 0) break;
                            //Get the closet available trading day with price
                            foreach (var item in filteredHistory)
                            {
                                if (DateTime.Compare(item.Date, date) >= 0)
                                {
                                    //result += item.Date.ToString() + "\n";
                                    shares += requestDTO.Amount / item.Open;
                                    buyCount++;
                                    //result += String.Format("{0,-30} - {1, -10} - {2,10} - {3,5}\n", item.Date.Date, Math.Round(item.Open, 4), Math.Round(requestDTO.Amount / item.Open, 4), buyCount);
                                    break;
                                }
                            }
                        }
                        iterator++;
                    }
                    invested = buyCount * requestDTO.Amount;
                    totalCurrentValue = shares * filteredHistory[^1].Open;
                    decimal percentChange = Math.Round(((totalCurrentValue - invested) / invested), 4);
                    result += String.Format("Increase / Decrease : {0:P2}.\n", percentChange);
                    result += String.Format("Results: ----------------------------------------------------------------- \n");
                    result += String.Format("{0, -20} - {1, 20}\n", "Before", "After");
                    result += String.Format("{0, -20} - {1, 20}\n", invested, totalCurrentValue);
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "failed!";
            }
        }
    }
}
