using HouseRentingSystem.Data;
using HouseRentingSystem.Models.Houses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

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
        public IActionResult Edit(int id) => View(new HouseFormModel());

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, HouseFormModel house)
        {
            return RedirectToAction(nameof(Details), new { id = "1" });
        }

        [Authorize]
        public IActionResult Add() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Add(HouseFormModel model)
        {
            return RedirectToAction(nameof(Details), new { id = "1" });
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
                .Where(h => h.Agent.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
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
    }
}