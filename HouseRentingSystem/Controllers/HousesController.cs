using HouseRentingSystem.Models.Houses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    public class HousesController : Controller
    {
        public IActionResult Details(int id)
        {
            return View(new House);
        }

        [Authorize]
        public IActionResult Mine()
        {
            return View(new AllHousesQueryModel());
        }
        public IActionResult All()
        {
            return View(new AllHousesQueryModel());
        }


    }
}