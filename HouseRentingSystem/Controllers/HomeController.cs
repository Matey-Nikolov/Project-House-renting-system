using HouseRentingSystem.Data;
using HouseRentingSystem.Models;
using HouseRentingSystem.Models.Houses;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HouseRentingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly HouseRentingDbContext  data;

        public HomeController(ILogger<HomeController> logger)
        {
            data = data;
        }

        public IActionResult Index()
        {
            var allHouses = new IndexViewModel()
            {
                Houses = Common.GetHouses()
            };

            return View(allHouses);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}