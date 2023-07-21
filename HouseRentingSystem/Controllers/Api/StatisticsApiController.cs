using HouseRentingSystem.Services.Statistics;
using HouseRentingSystem.Services.Statistics.Models;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers.Api
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IStatisticsService statistics;

        public StatisticsApiController(IStatisticsService statistics)
        {
            this.statistics = statistics;
        }

        [HttpGet]
        public StatisticsServiceModel GetStatistics()
            => statistics.Total();
    }
}