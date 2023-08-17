using HouseRentingSystem.Services.Statistics.Models;
using HouseRentingSystem.Web.Controllers.Api;

namespace HouseRentingSystem.Tests.IntegrationTests
{
    public class StatisticsApiControllerTests
    {
        private StatisticsApiController statisticsController;

        [OneTimeSetUp]
        public void SetUp()
        {
            statisticsController = new StatisticsApiController(StatisticsServiceMock.Instance);
        }

        [Test]
        public void GetStatistics_ShouldReturnCorrectCounts()
        {
            StatisticsServiceModel result = statisticsController.GetStatistics();
        
            Assert.NotNull(result);
            Assert.AreEqual(10, result.TotalHouses);
            Assert.AreEqual(6, result.TotalRents);
        }
    }
}