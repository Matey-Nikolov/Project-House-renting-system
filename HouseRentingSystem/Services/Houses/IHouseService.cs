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

        HouseDetailsServiceModel HouseDetailsById(int id);


        bool Exists(int id);
        
        bool CategoryExists(int categoryId);
       
        bool HasAgentWithId(int houseId, string currentUserId);
        
        bool IsRented(int id);
        bool IsRentedByUserWithId(int houseId, string userId);

        int Create(string title, string address,
            string description, string imageUrl,
            decimal price, int categoryId, int agentId);

        int GetHouseCategoryId(int houseId);

        void Edit(int houseId, string title, string address,
            string description, string imageUrl, decimal price, int categoryId);

        void Rent(int houseId, string userId);

        void Delete(int houseId);

        void Leave(int houseId);




        IEnumerable<string> AllCategoriesNames();
        IEnumerable<HouseCategoryServiceModel> AllCategories();
        IEnumerable<HouseServiceModel> AllHousesByAgentId(int agentId);
        IEnumerable<HouseServiceModel> AllHousesByUserId(string userId);
        IEnumerable<HouseIndexServiceModel> LastThreeHouses();
    }
}