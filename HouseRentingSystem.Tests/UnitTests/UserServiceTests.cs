using HouseRentingSystem.Services.Users;
using HouseRentingSystem.Services.Users.Models;

namespace HouseRentingSystem.Tests.UnitTests
{
    [TestFixture]
    public class UserServiceTests : UnitTestsBase
    {
        private IUserService userService;

        [OneTimeSetUp]
        public void StartUp()
        {
            userService = new UserService(data, mapper);
        }

        [Test]
        public void UserHasRents_ShouldReturnTrue_WithValidData()
        {
            bool result = userService.UserHasRents(Renter.Id);
        
            Assert.IsTrue(result);
        }

        [Test]
        public void UserFullName_ShouldReturnCorrectResult()
        {
            string result = userService.UserFullName(Renter.Id);

            string renterFullName = Renter.FirstName + " " + Renter.LastName;

            Assert.AreEqual(renterFullName, result);
        }

        [Test]
        public void All_ShouldReturnCorrectUsersAndAgents()
        {
            IEnumerable<UserServiceModel> result = userService.All();

            int usersCount = data.Users.Count();
            List<UserServiceModel> resultUsers = result.ToList();

            Assert.AreEqual (usersCount, resultUsers.Count());

            int agentsCount = data.Agents.Count();
            List<UserServiceModel> resultAgents = resultUsers
                .Where(us => us.PhoneNumber != "")
                .ToList();

            Assert.AreEqual(agentsCount, resultAgents.Count());

            UserServiceModel agentUser = resultAgents
                .FirstOrDefault(ag => ag.Email == Agent.User.Email);

            Assert.IsNotNull(agentUser);
            Assert.AreEqual(Agent.PhoneNumber, agentUser.PhoneNumber);
        }
    }
}