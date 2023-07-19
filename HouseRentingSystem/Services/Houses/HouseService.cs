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
    }
}