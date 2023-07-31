using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Entities;
using HouseRentingSystem.Models;
using HouseRentingSystem.Services.Agents.Models;
using HouseRentingSystem.Services.Houses.Models;

namespace HouseRentingSystem.Services.Houses
{
    public class HouseService : IHouseService
    {
        private readonly HouseRentingDbContext data;

        public HouseService(HouseRentingDbContext data)
            => this.data = data;

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
                .Select(h => new HouseServiceModel
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth
                })
                .ToList();

            var totalHouses = housesQuery.Count();

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
            => data
               .Houses
               .Where(h => h.Id == id)
                .Select(h => new HouseDetailsServiceModel()
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    Description = h.Description,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId != null,
                    Category = h.Category.Name,
                    Agent = new AgentServiceModel()
                    {
                        PhoneNumber = h.Agent.PhoneNumber,
                        Email = h.Agent.User.Email
                    }
                })
                .FirstOrDefault();

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

        private List<HouseServiceModel> ProjectToModel(List<House> houses)
        {
            List<HouseServiceModel> resultHouses = houses
                .Select(h => new HouseServiceModel()
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId == null
                })
                .ToList();

            return resultHouses;
        }

        public IEnumerable<string> AllCategoriesNames()
            => data.Categories
            .Select(c => c.Name)
            .Distinct()
            .ToList();

        public IEnumerable<HouseCategoryServiceModel> AllCategories()
            => data
            .Categories
            .Select(c => new HouseCategoryServiceModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList();

        public IEnumerable<HouseServiceModel> AllHousesByAgentId (int agentId)
        {
            List<House> houses = data
                .Houses
                .Where(h => h.AgentId == agentId)
                .ToList();

            return ProjectToModel(houses);
        }

        public IEnumerable<HouseServiceModel> AllHousesByUserId(string userId)
        {
            List<House> houses = data
                .Houses
                .Where(h => h.RenterId == userId)
                .ToList();

            return ProjectToModel(houses);
        }

        public IEnumerable<HouseIndexServiceModel> LastThreeHouses()
            => data
            .Houses
            .OrderByDescending(c => c.Id)
            .Select(c => new HouseIndexServiceModel
            {
                Id = c.Id,
                Title = c.Title,
                Address = c.Address,
                ImageUrl = c.ImageUrl
            })
            .Take(3);
    }
}