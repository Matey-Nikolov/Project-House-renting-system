using HouseRentingSystem.Models;
using HouseRentingSystem.Services.Houses.Models;

namespace HouseRentingSystem.Services.Houses
{
    public interface IHouseService
    {
        // Service methods
        HouseQueryServiceModel All
            (
                string category = null,
                string searchTerm = null,
                HouseSorting sorting = HouseSorting.Newest,
                int categoryPage = 1,
                int housesPerPage = 1
            );


        bool Exists(int id);

        HouseDetailsServiceModel HouseDetailsById(int id);

        // -------------

        //Iterators
        IEnumerable<string> AllCategoriesNames();
        IEnumerable<HouseServiceModel> AllHousesByAgentId(int agentId);
        IEnumerable<HouseServiceModel> AllHousesByUserId(string userId);
    }
}