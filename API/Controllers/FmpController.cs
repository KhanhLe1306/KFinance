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
        public FmpController(IConfiguration config)
        {
            _helper = new Helper(config);
        }

        [HttpGet("getROI")]
        public async Task<string> GetROI(RequestDTO requestDTO)
        {
            var historical = await _helper.GetHistorical(requestDTO);

            return _helper.CalculateROI(historical, requestDTO);
        }

        [HttpGet("getPossibility")]
        public async Task<string> GetPossibility(RequestDTO requestDTO)
        {
            var historical = await _helper.GetHistorical(requestDTO);

            return _helper.CalculatePosibility(historical, requestDTO);
        }
    }
}
