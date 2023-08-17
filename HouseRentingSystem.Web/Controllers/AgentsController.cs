using HouseRentingSystem.Web.Infrastructure;
using HouseRentingSystem.Web.Models.Agents;

using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Web.Controllers
{
    public class AgentsController : Controller
    {
        private readonly IAgentService agents;
        private readonly IUserService users;

        public AgentsController(IAgentService agents, IUserService users)
        {
            this.agents = agents;
            this.users = users;
        }

        [Authorize]
        public IActionResult Become()
        {
            if (agents.ExistsById(User.Id()))
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeAgentFormModel model)
        {
            string userId = User.Id();

            if (agents.ExistsById(userId))
                return BadRequest();

            if (agents.AgentWithPhoneNumberExists(model.PhoneNumber))
            {
                ModelState.AddModelError(nameof(model.PhoneNumber),
                    "Phone number already exists. Enter another one.");
            }

            if (users.UserHasRents(userId))
            {
                ModelState.AddModelError("Error",
                    "Ypu should have no rents to become an agent!");
            }

            if (!ModelState.IsValid)
                return View(model);

            agents.Create(userId, model.PhoneNumber);

            TempData["message"] = "You have successfully become an agent!";

            return RedirectToAction(nameof(HousesController.All), "Houses");
        }
        
    }
}