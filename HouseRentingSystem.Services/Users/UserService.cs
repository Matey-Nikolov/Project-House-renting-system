using AutoMapper;
using AutoMapper.QueryableExtensions;
using HouseRentingSystem.Services.Data;
using HouseRentingSystem.Services.Data.Entities;
using HouseRentingSystem.Services.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services.Users
{
    public class UserService : IUserService
    {
        private readonly HouseRentingDbContext data;
        private readonly IMapper mapper;

        public UserService(HouseRentingDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public string UserFullName(string userId)
        {
            User user = data.Users.Find(userId);

            if(string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName))
            {
                return null;
            }

            return user.FirstName + " " + user.LastName;
        }

        public bool UserHasRents(string userId)
        {
            return data.Houses.Any(h => h.RenterId == userId);
        }

        public IEnumerable<UserServiceModel> All()
        {
            List<UserServiceModel> allUsers = new List<UserServiceModel>();

            List<UserServiceModel> agents = data
                .Agents
                .Include(ag => ag.User)
                .ProjectTo<UserServiceModel>(mapper.ConfigurationProvider)
                .ToList();

            allUsers.AddRange(agents);

            List<UserServiceModel> users = data
                .Users
                .Where(u => !data.Agents.Any(ag => ag.UserId == u.Id))
                .ProjectTo<UserServiceModel>(mapper.ConfigurationProvider)
                .ToList();

            allUsers.AddRange(users);

            return allUsers;
        }
    }
}
