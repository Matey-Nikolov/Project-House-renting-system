using HouseRentingSystem.Services.Rents;
using HouseRentingSystem.Services.Rents.Models;

namespace HouseRentingSystem.Tests.UnitTests
{
    [TestFixture]
    public class RentServiceTests : UnitTestsBase
    {
        private IRentService rentService;

        [OneTimeSetUp]
        public void StartUp()
        {
            rentService = new RentService(data, mapper);
        }

        [Test]
        public void All_ShouldReturnCorrectData()
        {
            IEnumerable<RentServiceModel> result = rentService.All();
            Assert.IsNotNull(result);

            IEnumerable<House> rentedHousesInDb = data.Houses
                .Where(h => h.RenterId != null);

            Assert.AreEqual(rentedHousesInDb.Count(), result.ToList().Count());

            RentServiceModel resultHouse = result.ToList()
                .Find(h => h.HouseTitle == RentedHouse.Title);

            string renterFullName = Renter.FirstName + " " + Renter.LastName;
            string agentFullName = Agent.User.FirstName + " " + Agent.User.LastName;

            Assert.IsNotNull(resultHouse);
            Assert.AreEqual(Renter.Email, resultHouse.RenterEmail);
            Assert.AreEqual(renterFullName, resultHouse.RenterFullName);
            Assert.AreEqual(agentFullName, resultHouse.AgentFullName);
        }
    }
}