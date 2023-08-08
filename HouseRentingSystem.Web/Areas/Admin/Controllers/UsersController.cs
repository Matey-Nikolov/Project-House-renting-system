using HouseRentingSystem.Services.Users;
using HouseRentingSystem.Services.Users.Models;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
    public class UsersController : AdminController
    {
        private readonly IUserService users;

        public UsersController(IUserService users) 
        { 
            this.users = users;
        }

        [Route("Users/All")]
        public IActionResult All()
        {
            IEnumerable<UserServiceModel> users = this.users.All();
            return View(users);
        }
    }
}