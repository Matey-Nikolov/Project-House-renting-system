using HouseRentingSystem.Services.Users;
using HouseRentingSystem.Services.Users.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
    public class UsersController : AdminController
    {
        private readonly IUserService users;
        private readonly IMemoryCache cache;

        public UsersController(IUserService users, IMemoryCache cache) 
        { 
            this.users = users;
            this.cache = cache;
        }

        [Route("Users/All")]
        public IActionResult All()
        {
            IEnumerable<UserServiceModel> users = cache.Get<IEnumerable<UserServiceModel>>("UsersCacheKey");

            if (users == null)
            {
                users = this.users.All();

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                cache.Set("UsersCacheKey", users, cacheOptions);
            }

            return View(users);
        }
    }
}