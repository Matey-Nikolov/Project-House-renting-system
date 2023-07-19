using HouseRentingSystem.Models;
using HouseRentingSystem.Services.Houses.Models;
using System.Security.Cryptography.Xml;

namespace HouseRentingSystem.Services.Houses
{
    public interface IHouseService
    {
        HouseQueryServiceModel All
            (
                string category = null,
                string searchTerm = null,
                HouseSorting sorting = HouseSorting.Newest,
                int categoryPage = 1,
                int housesPerPage = 1
            );

        bool Exists(int id);
        bool CategoryExists(int categoryId);

        int Create(string title, string address,
            string description, string imageUrl,
            decimal price, int categoryId, int agentId);

        HouseDetailsServiceModel HouseDetailsById(int id);

        IEnumerable<string> AllCategoriesNames();
        IEnumerable<HouseCategoryServiceModel> AllCategories();
        IEnumerable<HouseServiceModel> AllHousesByAgentId(int agentId);
        IEnumerable<HouseServiceModel> AllHousesByUserId(string userId);
    }
}