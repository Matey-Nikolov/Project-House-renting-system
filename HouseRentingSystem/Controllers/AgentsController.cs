using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    public class AgentsController : Controller
    {
        [HttpPost]
        [Authorize]
        public IActionResult Become()
        {
            return RedirectToAction(nameof(HousesController.All), "Houses");
        }
    }
}