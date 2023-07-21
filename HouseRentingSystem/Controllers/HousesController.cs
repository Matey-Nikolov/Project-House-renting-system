using HouseRentingSystem.Models.Houses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
using HouseRentingSystem.Infrastructure;
using HouseRentingSystem.Services.Houses;
using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Houses.Models;

namespace HouseRentingSystem.Controllers
{
    public class HousesController : Controller
    {
 
        private readonly IHouseService houses;
        private readonly IAgentService agents;

        public HousesController(IHouseService houses, IAgentService agents)
        {
            this.houses = houses;
            this.agents = agents;
        }


        [HttpPost]
        [Authorize]
        public IActionResult Leave(int id)
        {
            if (!houses.Exists(id) || !houses.IsRented(id))
                return BadRequest();

            if (!houses.IsRentedByUserWithId(id, User.Id()))
                return Unauthorized();
            
            houses.Leave(id);

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Rent(int id)
        {
            if (!houses.Exists(id))
                return BadRequest();

            if (agents.ExistsById(User.Id()))
                return Unauthorized();

            if (houses.IsRented(id))
                return BadRequest();

            houses.Rent(id, User.Id());

            return RedirectToAction(nameof(Mine));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!houses.Exists(id))
                return BadRequest();

            if (!houses.HasAgentWithId(id, User.Id()))
                return Unauthorized();

            var house = houses.HouseDetailsById(id);

            int houseCategoryId = houses.GetHouseCategoryId(house.Id);

            HouseFormModel houseModel = new HouseFormModel()
            {
                Title = house.Title,
                Address = house.Address,
                Description = house.Description,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                CategoryId = houseCategoryId,
                Categories = houses.AllCategories()
            };

            return View(houseModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, HouseFormModel model)
        {
            if (!houses.Exists(id))
                return View();

            if (!houses.HasAgentWithId(id, User.Id()))
                return Unauthorized();

            if (!houses.CategoryExists(model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId),
                    "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = houses.AllCategories();

                return View(model);
            }

            houses.Edit(id, model.Title, model.Address, model.Description,
                model.ImageUrl, model.PricePerMonth, model.CategoryId);

            return RedirectToAction(nameof(Details), new { id = id });
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!agents.ExistsById(User.Id()))
                return RedirectToAction(nameof(AgentsController.Become), "Agents");

            return View(new HouseFormModel
            {
                Categories = houses.AllCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(HouseFormModel model)
        {
            if (!agents.ExistsById(User.Id()))
                return RedirectToAction(nameof(AgentsController.Become), "Agents");

            if (!houses.CategoryExists(model.CategoryId))
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist.");

            if (!ModelState.IsValid)
            {
                model.Categories = houses.AllCategories();
                return View(model);
            }

            int agentId = agents.GetAgentId(User.Id());

            int newHouseId = houses.Create(model.Title, model.Address, model.Description,
                model.ImageUrl, model.PricePerMonth, model.CategoryId, agentId);

            return RedirectToAction(nameof(Details), new { id = newHouseId });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!houses.Exists(id))
                return BadRequest();

            if (!houses.HasAgentWithId(id, User.Id()))
                return Unauthorized();

            var house = houses.HouseDetailsById(id);

            HouseDetailsViewModel model = new HouseDetailsViewModel()
            {
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(HouseDetailsViewModel model)
        {
            if (!houses.Exists(model.Id))
                return BadRequest();

            if (!houses.HasAgentWithId(model.Id, User.Id()))
                return Unauthorized();

            houses.Delete(model.Id);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int id)
        {
            if (!houses.Exists(id))
                return BadRequest();

            HouseDetailsServiceModel houseModel = houses.HouseDetailsById(id);

            return View(houseModel);
        }

        [Authorize]
        public IActionResult Mine()
        {
            IEnumerable<HouseServiceModel> myHouses = null;

            string userId = User.Id();

            if (agents.ExistsById(userId))
            {
                int currentAgentId = agents.GetAgentId(userId);
                myHouses = houses.AllHousesByAgentId(currentAgentId);
            }
            else
            {
                myHouses = houses.AllHousesByUserId(userId);
            }

            return View(myHouses);
        }

        public IActionResult All([FromQuery] AllHousesQueryModel query)
        {
            var queryResult = houses.All
                (
                    query.Category,
                    query.SearchTerm,
                    query.Sorting,
                    query.CurrentPage,
                    AllHousesQueryModel.HousesPerPage
                );

            query.TotalHousesCount = queryResult.TotalHousesCount;
            query.Houses = queryResult.Houses;

            var houseCategories = houses.AllCategoriesNames();
            query.Categories = houseCategories;

            return View(query);
        }
    }
}