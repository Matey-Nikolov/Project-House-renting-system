using HouseRentingSystem.Data;
using HouseRentingSystem.Models.Houses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
//using System.Security.Claims;
using HouseRentingSystem.Infrastructure;
using HouseRentingSystem.Models.Category;
using HouseRentingSystem.Data.Entities;
using HouseRentingSystem.Models;
using HouseRentingSystem.Models.Agents;
using HouseRentingSystem.Services.Houses;
using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Houses.Models;

namespace HouseRentingSystem.Controllers
{
    public class HousesController : Controller
    {
        private readonly HouseRentingDbContext data;
        
        private readonly IHouseService houses;
        private readonly IAgentService agents;

        public HousesController(HouseRentingDbContext data, IHouseService houses, IAgentService agents)
        {
            this.data = data;
            this.houses = houses;
            this.agents = agents;
        }


        [HttpPost]
        [Authorize]
        public IActionResult Leave(int id)
        {
            if (!data.Houses.Any(h => h.Id == id && h.RenterId != null))
            {
                return BadRequest();
            }

            var house = data.Houses.Find(id);

            if (house.RenterId != User.Id())
            {
                return Unauthorized();
            }

            house.RenterId = null;

            data.SaveChanges();

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Rent(int id)
        {
            if (!data.Houses.Any(h => h.Id == id))
            {
                return BadRequest();
            }

            if (data.Agents.Any(a => a.UserId == User.Id()))
            {
                return Unauthorized();
            }

            var house = data.Houses.Find(id);

            if (house.RenterId != null)
            {
                return BadRequest();
            }

            house.RenterId = User.Id();

            data.SaveChanges();

            return RedirectToAction(nameof(Mine));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var house = data.Houses.Find(id);

            if (house == null)
            {
                return BadRequest();
            }

            var agent = data.Agents.FirstOrDefault(a => a.Id == house.AgentId);

            if (agent == null || agent.UserId != User.Id())
            {
                return Unauthorized();
            }

            HouseFormModel houseModel = new HouseFormModel()
            {
                Title = house.Title,
                Address = house.Address,
                Description = house.Description,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                CategoryId = house.CategoryId,
                Categories = GetHouseCategories()
            };

            return View(houseModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, HouseFormModel model)
        {
            var house = data.Houses.Find(id);

            if (house == null)
            {
                return View();
            }

            var agent = data.Agents.FirstOrDefault(a => a.Id == house.AgentId);

            if (agent == null || agent.UserId != User.Id())
            {
                return Unauthorized();
            }

            if (!data.Categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId),
                    "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = GetHouseCategories();

                return View(model);
            }

            house.Title = model.Title;
            house.Address = model.Address;
            house.Description = model.Description;
            house.ImageUrl = model.ImageUrl;
            house.PricePerMonth = model.PricePerMonth;
            house.CategoryId = model.CategoryId;

            data.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = "1" });
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!data.Agents.Any(a => a.UserId == User.Id()))
            {
                return RedirectToAction(nameof(AgentsController.Become), "Agents");
            }

            return View(new HouseFormModel
            {
                Categories = GetHouseCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(HouseFormModel model)
        {
            if (!this.data.Agents.Any(a => a.UserId == this.User.Id()))
            {
                return RedirectToAction(nameof(AgentsController.Become), "Agents");
            }

            if (!this.data.Categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId),
                    "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = this.GetHouseCategories();

                return View(model);
            }

            var agentId = this.data.Agents
                .FirstOrDefault(a => a.UserId == this.User.Id())
                .Id;

            var house = new House
            {
                Title = model.Title,
                Address = model.Address,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth,
                CategoryId = model.CategoryId,
                AgentId = agentId
            };

            this.data.Houses.Add(house);
            this.data.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = house.Id });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var house = data.Houses.Find(id);

            if (house == null)
            {
                return BadRequest();
            }

            var agent = data.Agents.FirstOrDefault(a => a.Id == house.AgentId);

            if (agent == null || agent.UserId != User.Id())
            {
                return Unauthorized();
            }

            HouseViewModel model = new HouseViewModel()
            {
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(HouseViewModel model)
        {
            var house = data.Houses.Find(model.Id);

            if (house == null)
            {
                return BadRequest();
            }

            var agent = data.Agents.FirstOrDefault(a => a.Id == house.AgentId);

            if (agent == null || agent.UserId != User.Id())
            {
                return Unauthorized();
            }

            data.Houses.Remove(house);
            data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int id)
        {
            if (!houses.Exists(id))
            {
                return BadRequest();
            }

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

        private IEnumerable<HouseCategoryViewModel> GetHouseCategories()
        {
            return data.Categories
                .Select(c => new HouseCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }

    }
}