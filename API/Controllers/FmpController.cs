using KFinance.DTOs;
using Microsoft.AspNetCore.Mvc;
using KFinance.Utils;

namespace KFinance.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class FmpController : Controller
    {
        private readonly Helper _helper;
        private readonly HttpClient client = new()
        {
            BaseAddress = new Uri("https://financialmodelingprep.com/api/v3"),
        };
        private readonly IConfiguration _config;
        public FmpController(IConfiguration config)
        {
            _config = config;
            _helper = new Helper();
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
                List<History> historical = res.Historical;
                historical.Sort();

                //return _helper.CalculatePercentageChange(historical, requestDTO);


                return _helper.MachineLearning(historical, requestDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.Message.ToString();
            }
        }
    }
}
