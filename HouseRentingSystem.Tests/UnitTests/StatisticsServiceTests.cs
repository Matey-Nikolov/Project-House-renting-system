using HouseRentingSystem.Services.Statistics;
using HouseRentingSystem.Services.Statistics.Models;

namespace HouseRentingSystem.Tests.UnitTests
{
    [TestFixture]
    public class StatisticsServiceTests : UnitTestsBase
    {
        private IStatisticsService statisticsService;

        [OneTimeSetUp]
        public void SetUp()
        {
            statisticsService = new StatisticsService(data);
        }

        [Test]
        public void Total_ShouldReturnCorrectCounts()
        {
            StatisticsServiceModel result = statisticsService.Total();
            Assert.IsNotNull(result);

            int housesCount = data.Houses.Count();
            Assert.AreEqual(housesCount, result.TotalHouses);

            int rentsCount = data.Houses.Where(h => h.RenterId != null).Count();
            Assert.AreEqual(rentsCount, result.TotalRents);
        }
    }
}