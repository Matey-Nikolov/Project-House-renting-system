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
    }
}