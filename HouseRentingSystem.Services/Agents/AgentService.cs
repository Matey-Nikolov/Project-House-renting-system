﻿using HouseRentingSystem.Services.Data;
using HouseRentingSystem.Services.Data.Entities;

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

        public bool AgentWithPhoneNumberExists(string phoneNumber)
            => data.Agents.Any(a => a.PhoneNumber == phoneNumber);

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