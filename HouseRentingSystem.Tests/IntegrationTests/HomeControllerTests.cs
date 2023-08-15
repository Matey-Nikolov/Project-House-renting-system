using HouseRentingSystem.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Tests.IntegrationTests
{
    public class HomeControllerTests
    {
        private HomeController homeController;

        [OneTimeSetUp]
        public void SetUp()
        {
            homeController = new HomeController(null);
        }

        [Test]
        public void Error_ShouldReturnCorrectView()
        {
            int statusCode = 500;

            // IActionResult result = homeController.Error(statusCode);
             var result = homeController.Error(statusCode);
            Assert.IsNotNull(result);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }
    }
}