using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Entities;
using HouseRentingSystem.Infrastructure;
using HouseRentingSystem.Models.Agents;
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

        [Authorize]
        public IActionResult Become()
        {
            if (data.Agents.Any(a => a.UserId == User.Id()))
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeAgentFormModel model)
        {
            if (data.Agents.Any(a => a.UserId == User.Id()))
            {
                return BadRequest();
            }

            if (data.Agents.Any(a => a.PhoneNumber == model.PhoneNumber))
            {
                ModelState.AddModelError(nameof(model.PhoneNumber),
                    "Phone number already exists. Enter another one.");
            }

            if (data.Houses.Any(h => h.RenterId == User.Id()))
            {
                ModelState.AddModelError("Error",
                    "Ypu should have no rents to become an agent!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var agent = new Agent()
            {
                UserId = User.Id(),
                PhoneNumber = model.PhoneNumber
            };

            data.Agents.Add(agent);
            data.SaveChanges();

            return RedirectToAction(nameof(HousesController.All), "Houses");
        }
        
    }
}