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
                    int count = 0;
                    while (true)
                    {
                        if (count % requestDTO.Frequency == 0)
                        {
                            DateTime date = requestDTO.Start.AddDays(count);
                            if (DateTime.Compare(date, filteredHistory[^1].Date) > 0) break;
                            //Get the closet available trading day with price
                            foreach (var item in filteredHistory)
                            {
                                if (DateTime.Compare(item.Date, date) >= 0)
                                {
                                    result += item.Date.ToString() + "\n";
                                    break;
                                }
                            }
                        }
                        count++;
                    }
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
