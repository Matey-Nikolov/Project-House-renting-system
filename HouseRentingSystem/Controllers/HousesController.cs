using HouseRentingSystem.Data;
using HouseRentingSystem.Models.Houses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
//using System.Security.Claims;
using HouseRentingSystem.Infrastructure;
using HouseRentingSystem.Models.Category;
using HouseRentingSystem.Data.Entities;

namespace HouseRentingSystem.Controllers
{
    public class HousesController : Controller
    {
        private readonly HouseRentingDbContext data;
        public HousesController(HouseRentingDbContext data)
             => this.data = data;

        [HttpPost]
        [Authorize]
        public IActionResult Leave(int id)
        {
            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Rent(int id)
        {
            return RedirectToAction(nameof(Mine));
           //return RedirectToAction(Mine());
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

            return View();
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

            if (ModelState.IsValid)
            {
                model.Categories = GetHouseCategories();

                return View(model);
            }

            house.Title = model.Title; 
            house.Address = model.Address;
            house.Description = model.Description;
            house.ImageUrl = model.ImageUrl;
            house.PricePerMonth= model.PricePerMonth;
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
            if (!data.Agents.Any(a => a.UserId == User.Id()))
            {
                return RedirectToAction(nameof(AgentsController.Become), "Agents");
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

            var agentId = data.Agents
                .FirstOrDefault(a => a.UserId == User.Id())
                .Id;

            House house = new House
            {
                Title = model.Title,
                Address= model.Address,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth,
                CategoryId = model.CategoryId,
                AgentId = agentId
            };

            data.Houses.Add(house);
            data.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = house.Id });
        }

        [Authorize]
        public IActionResult Delete(int id) => View(new HouseDetailsViewModel());

        [HttpPost]
        [Authorize]
        public IActionResult Delete(HouseDetailsViewModel house)
        {
            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int id)
        {
            var house = data.Houses.Find(id);

            if (house == null)
            {
                return BadRequest();
            }

            var houseModel = new HouseDetailsViewModel()
            {
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl
            };

            return View(houseModel);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var allHouses = new AllHousesQueryModel()
            {
                Houses = data.Houses
                .Where(h => h.Agent.UserId == User.Id())
                .Select(h => new HouseViewModel()
                { 
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl
                })
            };

            return View(allHouses);
        }

        public IActionResult All()
        {
            var allHouses = new AllHousesQueryModel()
            {
                Houses = data.Houses
                .Select(h => new HouseViewModel()
                {
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl
                })
            };

            return View(allHouses);
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