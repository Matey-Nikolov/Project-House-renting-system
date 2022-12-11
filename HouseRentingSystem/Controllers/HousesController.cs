using HouseRentingSystem.Models.Houses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HouseRentingSystem.Controllers
{
    public class HousesController : Controller
    {
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

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, HouseFormModel house)
        {
            // !!  ! !!  !! ! ! ! ! 
            return RedirectToAction(nameof(Details), new { id = "1" });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(HouseFormModel model)
        {
            //    !!!!!!!
            return RedirectToAction(nameof(Details), new {id = "1"});
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(HouseFormModel house)
        {
            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int id)
        {
            var house = Common.GetHouses().FirstOrDefault();

            return View(house);
        }

        [Authorize]
        public IActionResult Mine()
        {
            return View(new AllHousesQueryModel());
        }

        //[Authorize]
        //public IActionResult All()
        //{

        //}

        public IActionResult All()
        {
            var allHouses = new AllHousesQueryModel()
            {
                Houses = Common.GetHouses()
            };

            return View(allHouses);
        }
    }
}