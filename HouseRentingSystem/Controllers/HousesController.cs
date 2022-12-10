using HouseRentingSystem.Models.Houses;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    public class HousesController : Controller
    {
        public IActionResult Mine()
        {
            return View();
        }
        public IActionResult All()
        {
            return View(new AllHousesQueryModel());
        }


    }
}