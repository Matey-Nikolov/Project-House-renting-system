using HouseRentingSystem.Services.Rents;
using HouseRentingSystem.Services.Rents.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
    public class RentsController : AdminController
    {
        private readonly IRentService rents;
        private readonly IMemoryCache cache;

        public RentsController(IRentService rents, IMemoryCache cache)
        {
            this.rents = rents;
            this.cache = cache;
        }

        [Route("Rents/All")]
        public IActionResult All()
        {
            IEnumerable<RentServiceModel> rents = cache.Get<IEnumerable<RentServiceModel>>("RentsCacheKey");

            if (rents != null)
            {
                rents = this.rents.All();

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                cache.Set("RentsCacheKey", rents, cacheOptions);
            }

            return View(rents);
        }
    }
}