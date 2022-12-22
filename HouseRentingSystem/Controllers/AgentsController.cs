using HouseRentingSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    public class AgentsController : Controller
    {
        private readonly HouseRentingDbContext data;


        public AgentsController(HouseRentingDbContext data)
        {
            this.data = data;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Become()
        {
            return RedirectToAction(nameof(HousesController.All), "Houses");
        }
    }
}