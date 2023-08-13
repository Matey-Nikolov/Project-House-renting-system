using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Houses;

using HouseRentingSystem.Web.Areas.Admin.Models;
using HouseRentingSystem.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
    public class HousesController : AdminController
    {
        private readonly IHouseService houses;
        private readonly IAgentService agents;

        public HousesController(IHouseService houses, IAgentService agents)
        {
            this.houses = houses;
            this.agents = agents;
        }

        public IActionResult Mine()
        {
            MyHousesViewModel myHouses = new MyHousesViewModel();

            string adminUserId = User.Id();
            myHouses.RentedHouses = houses.AllHousesByUserId(adminUserId);

            int adminAgentId = agents.GetAgentId(adminUserId);
            myHouses.AddedHouses = houses.AllHousesByAgentId(adminAgentId);

            return View(myHouses);
        }
    }
}