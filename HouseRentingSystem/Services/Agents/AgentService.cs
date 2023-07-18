using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Entities;

namespace HouseRentingSystem.Services.Agents
{
    public class AgentService : IAgentService
    {
        private readonly HouseRentingDbContext data;

        public AgentService(HouseRentingDbContext data)
            => this.data = data;

        public int GetAgentId(string userId)
            => data.Agents
            .FirstOrDefault(a => a.UserId == userId)
            .Id;

        public bool ExistsById(string userId)
            => data.Agents.Any(a => a.UserId == userId);

        public bool UserWithPhoneNumberExists(string phoneNumber)
            => data.Agents.Any(a => a.PhoneNumber == phoneNumber);

        public bool UserHasRents(string userId)
            => data.Houses.Any(h => h.RenterId == userId);

        public void Create(string userId, string phoneNumber)
        {
            Agent agent = new Agent()
            {
                UserId = userId,
                PhoneNumber = phoneNumber
            };

            data.Agents.Add(agent);
            data.SaveChanges();
        }
    }
}