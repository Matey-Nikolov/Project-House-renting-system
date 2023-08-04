using HouseRentingSystem.Services.Data;
using HouseRentingSystem.Services.Data.Entities;
using HouseRentingSystem.Services.Agents.Models;
using HouseRentingSystem.Services.Houses.Models;
using HouseRentingSystem.Services.Users;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services.Houses
{
    public class HouseService : IHouseService
    {
        private readonly HouseRentingDbContext data;
        private readonly IUserService users;
        private readonly IMapper mapper;

        public HouseService(HouseRentingDbContext data, IUserService users, IMapper mapper)
        {
            this.data = data;
            this.users = users;
            this.mapper = mapper;
        }


        public HouseQueryServiceModel All
            (
                string category = null,
                string searchTerm = null,
                HouseSorting sorting = HouseSorting.Newest,
                int currentPage = 1,
                int housesPerPage = 1
            )
        {
            var housesQuery = data.Houses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                housesQuery = data.Houses
                    .Where(h => h.Category.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                housesQuery = housesQuery
                    .Where(h =>
                    h.Title.ToLower().Contains(searchTerm.ToLower()) ||
                    h.Address.ToLower().Contains(searchTerm.ToLower()) ||
                    h.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            housesQuery = sorting switch
            {
                HouseSorting.Price => housesQuery
                .OrderBy(h => h.PricePerMonth),

                HouseSorting.NotRentedFirst => housesQuery
                .OrderBy(h => h.RenterId != null)
                .ThenByDescending(h => h.Id),
                _ => housesQuery.OrderByDescending(h => h.Id)
            };

            var houses = housesQuery
                .Skip((currentPage - 1) * housesPerPage)
                .Take(housesPerPage)
                .ProjectTo<HouseServiceModel>(mapper.ConfigurationProvider)
                .ToList();

            int totalHouses = housesQuery.Count();

            return new HouseQueryServiceModel()
            {
                TotalHousesCount = totalHouses,
                Houses = houses
            };
        }

        public bool Exists(int id)
            => data.Houses.Any(h => h.Id == id);

        public bool CategoryExists(int categoryId)
            => data.Categories.Any(c => c.Id == categoryId);

        public int GetHouseCategoryId(int houseId)
            => data.Houses.Find(houseId).CategoryId;

        public bool IsRented(int id)
         => data.Houses.Find(id).RenterId != null;

        public HouseDetailsServiceModel HouseDetailsById(int id)
        {
            House dbHouse = data
                .Houses
                .Include(h => h.Category)
                .Include(h => h.Agent.User)
                .Where(h => h.Id == id)
                .FirstOrDefault();

            HouseDetailsServiceModel house = mapper.Map<HouseDetailsServiceModel>(dbHouse);

            AgentServiceModel agent = mapper.Map<AgentServiceModel>(dbHouse.Agent);
            agent.FullName = users.UserFullName(dbHouse.Agent.UserId);

            house.Agent = agent;

            return house;
        }

        public bool HasAgentWithId(int houseId, string currentUserId)
        {
            var house = data.Houses.Find(houseId);
            var agent = data.Agents.FirstOrDefault(a => a.Id == house.AgentId);

            if (agent == null)
            {
                return false;
            }

            if (agent.UserId != currentUserId)
            {
                return false;
            }

            return true;
        }

        public int Create(string title, string address,
                        string description, string imageUrl,
                        decimal price, int categoryId, int agentId)
        {
            House house = new House
            {
                Title = title,
                Address = address,
                Description = description,
                ImageUrl = imageUrl,
                PricePerMonth = price,
                CategoryId = categoryId,
                AgentId = agentId
            };

            data.Houses.Add(house);
            data.SaveChanges();

            return house.Id;
        }

        public void Edit(int houseId, string title, string address, string description,
            string imageUrl, decimal price, int categoryId)
        {
            var house = data.Houses.Find(houseId);

            house.Title = title;
            house.Address = address;
            house.Description = description;
            house.ImageUrl = imageUrl;
            house.PricePerMonth = price;
            house.CategoryId = categoryId;

            data.SaveChanges();
        }

        public void Delete(int houseId)
        {
            var house = data.Houses.Find(houseId);

            data.Remove(house);
            data.SaveChanges();
        }

        public bool IsRentedByUserWithId(int houseId, string userId)
        {
            var house = data.Houses.Find(houseId);

            if (house == null)
                return false;

            if (house.RenterId != userId)
                return false;

            return true;
        }

        public void Rent(int houseId, string userId)
        {
            var house = data.Houses.Find(houseId);

            house.RenterId = userId;
            data.SaveChanges();
        }

        public void Leave(int houseId)
        {
            var house = data.Houses.Find(houseId);

            house.RenterId = null;
            data.SaveChanges();
        }

        public IEnumerable<string> AllCategoriesNames()
            => data.Categories
            .Select(c => c.Name)
            .Distinct()
            .ToList();

        public IEnumerable<HouseCategoryServiceModel> AllCategories()
            => data
            .Categories
            .ProjectTo<HouseCategoryServiceModel>(mapper.ConfigurationProvider)
            .ToList();

        public IEnumerable<HouseServiceModel> AllHousesByAgentId (int agentId)
        {
            List<HouseServiceModel> houses = data
                .Houses
                .Where(h => h.AgentId == agentId)
                .ProjectTo<HouseServiceModel>(mapper.ConfigurationProvider)
                .ToList();

            return houses;
        }

        public IEnumerable<HouseServiceModel> AllHousesByUserId(string userId)
        {
            List<HouseServiceModel> houses = data
                .Houses
                .Where(h => h.RenterId == userId)
                .ProjectTo<HouseServiceModel>(mapper.ConfigurationProvider)
                .ToList();

            return houses;
        }

        public IEnumerable<HouseIndexServiceModel> LastThreeHouses()
            => data.Houses
                .OrderByDescending(c => c.Id)
                .ProjectTo<HouseIndexServiceModel>(mapper.ConfigurationProvider)
                .Take(3);
    }
}