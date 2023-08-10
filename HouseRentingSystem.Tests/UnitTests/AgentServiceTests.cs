using HouseRentingSystem.Services.Agents;

namespace HouseRentingSystem.Tests.UnitTests
{
    [TestFixture]
    public class AgentServiceTests :  UnitTestsBase
    {
        private IAgentService agentService;

        [OneTimeSetUp]
        public void SetUp()
        {
            agentService = new AgentService(data);
        }

        [Test]
        public void CreateAgent_ShouldWorkCorrectly()
        {
            int agentsCountBefore = data.Agents.Count();

            agentService.Create(Agent.UserId, Agent.PhoneNumber);

            int agentsCountAfter = data.Agents.Count();

            Assert.AreEqual(agentsCountBefore + 1, agentsCountAfter);

            int newAgentId = agentService.GetAgentId(Agent.UserId);
            Agent newAgentInDb = data.Agents.Find(newAgentId);

            Assert.IsNotNull(newAgentInDb);
            Assert.AreEqual(Agent.UserId, newAgentInDb.UserId);
            Assert.AreEqual(Agent.PhoneNumber, newAgentInDb.PhoneNumber);
        }

        [Test]
        public void AgentWithPhoneNumberExists_ShouldReturnTrue_WithValidData()
        {
            bool result = agentService.AgentWithPhoneNumberExists(Agent.PhoneNumber);
        
            Assert.IsTrue(result);
        }

        [Test]
        public void ExistsById_ShouldReturnTrue_WithValidId()
        {
            bool result = agentService.ExistsById(Agent.UserId);

            Assert.IsTrue(result);
        }

        [Test]
        public void GetAgentId_ShouldReturnCorrectUserId() 
        {
            int resultAgentId = agentService.GetAgentId(Agent.UserId);

            Assert.AreEqual(Agent.Id, resultAgentId);
        }
    }
}