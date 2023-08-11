using HouseRentingSystem.Services.Houses;
using HouseRentingSystem.Services.Houses.Models;
using HouseRentingSystem.Services.Users;

namespace HouseRentingSystem.Tests.UnitTests
{
    [TestFixture]
    public class HouseServiceTests : UnitTestsBase
    {
        private IUserService userService;
        private IHouseService houseService;

        [OneTimeSetUp]
        public void SetUp()
        {
            userService = new UserService(data, mapper);
            houseService = new HouseService(data, userService, mapper);
        }

        [Test]
        public void Leave_ShouldRentHouseSuccessfully()
        {
            House house = new House()
            {
                Title = "New House for leave",
                RenterId = "TestRenterId",
                Address = "Add because is required - test leave",
                Description = "Add because is required - test leave",
                ImageUrl = "Add because is required - test leave"
            };

            data.Houses.Add(house);
            data.SaveChanges();

            houseService.Leave(house.Id);

            Assert.IsNull(house.RenterId);

            House newHouseInDb = data.Houses.Find(house.Id);

            Assert.IsNotNull(newHouseInDb);
            Assert.IsNull(newHouseInDb.RenterId);
        }

        [Test]
        public void LastThreeHouses_ShouldReturnCorrectHouses()
        {
            IEnumerable<HouseIndexServiceModel> result = houseService.LastThreeHouses();

            IEnumerable<House> housesInDb = data.Houses
                .OrderByDescending(h => h.Id)
                .Take(3);

            Assert.AreEqual(housesInDb.Count(), result.Count());

            House firstHouseInDb = housesInDb.FirstOrDefault();
            HouseIndexServiceModel firstResultHouse = result.FirstOrDefault();

            Assert.AreEqual(firstHouseInDb.Id, firstResultHouse.Id);
            Assert.AreEqual(firstHouseInDb.Title, firstResultHouse.Title);
        }

        [Test]
        public void Rent_ShouldRentHouseSuccessfully()
        {
            House house = new House()
            {
                Title = "New House for rent",
                Address = "Add because is required - test rent",
                Description = "Add because is required - test rent",
                ImageUrl = "Add because is required - test rent"
            };

            data.Houses.Add(house);
            data.SaveChanges();

            string renterId = Renter.Id;

            houseService.Rent(house.Id, renterId);

            var newHouseInDb = data.Houses.Find(house.Id);

            Assert.IsNotNull(newHouseInDb);
            Assert.AreEqual(house.RenterId, renterId);
        }

        [Test]
        public void IsRented_ShouldReturnCorrectTrue_WithValidId()
        {
            int houseId = RentedHouse.Id;
            bool result = houseService.IsRented(houseId);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsRentedByUserWithId_ShouldReturnCorrectTrue_WithValidId()
        {
            int houseId = RentedHouse.Id;
            string renterId = RentedHouse.Renter.Id;

            bool result = houseService.IsRentedByUserWithId(houseId, renterId);

            Assert.IsTrue(result);
        }

        [Test]
        public void Delete_ShouldDeleteHouseSuccessfully()
        {
            House house = new House()
            {
                Title = "New House for delete",
                Address = "Sofia",
                Description = "Add because is required - test delete",
                ImageUrl = "Add because is required - test delete"
            };

            data.Houses.Add(house);
            data.SaveChanges();

            int housesCountBefore = data.Houses.Count();

            houseService.Delete(house.Id);

            int housesCountAfter = data.Houses.Count();

            Assert.AreEqual(housesCountBefore - 1, housesCountAfter);

            House houseInDb = data.Houses.Find(house.Id);

            Assert.IsNull(houseInDb);
        }

        [Test]
        public void Edit_ShouldEditHouseCorrectly()
        {
            House house = new House()
            {
                Title = "New House for Edit",
                Address = "Sofia",
                Description = "Add because is required - test edit",
                ImageUrl = "Add because is required - test edit"
            };

            data.Houses.Add(house);
            data.SaveChanges();

            string changedAddress = "Sofia, Bulgaria";

            houseService.Edit(house.Id, house.Title, changedAddress, house.Description, house.ImageUrl, house.PricePerMonth, house.CategoryId);

            House newHouseInDb = data.Houses.Find(house.Id);

            Assert.IsNotNull(newHouseInDb);
            Assert.AreEqual(house.Title, newHouseInDb.Title);
            Assert.AreEqual(changedAddress, newHouseInDb.Address);
        }

        [Test]
        public void GetHouseCategoryId_ShouldReturnCorrectId()
        {
            int houseId = RentedHouse.Id;
            int result = houseService.GetHouseCategoryId(houseId);

            Assert.IsNotNull(result);

            int categoryId = RentedHouse.Category.Id;

            Assert.AreEqual(categoryId, result);
        }

        [Test]
        public void HasAgentWithId_ShouldReturnTrue_WithValidId()
        {
            int houseId = RentedHouse.Id;
            string userId = RentedHouse.Agent.User.Id;

            bool result = houseService.HasAgentWithId(houseId, userId);

            Assert.IsTrue(result);
        }

        [Test]
        public void Create_ShouldCreateHouse()
        {
            int housesInDbBefore = data.Houses.Count();

            House newHouse = new House()
            {
                Title = "New House"
            };

            int newHouseId = houseService.Create(newHouse.Title, "Add because is required", "Add because is required", "Add because is required", 2200.00M, 1, Agent.Id);

            int housesInDbAfter = data.Houses.Count();
            Assert.AreEqual(housesInDbBefore + 1, housesInDbAfter);

            House newHouseInDb = data.Houses.Find(newHouseId);
            Assert.AreEqual(newHouse.Title, newHouseInDb.Title);
        }

        [Test]
        public void CategoryExists_ShouldReturnTrue_WithValidId()
        {
            int categoryId = data.Categories.FirstOrDefault().Id;
            bool result = houseService.CategoryExists(categoryId);

            Assert.IsTrue(result);
        }

        [Test]
        public void AllCategories_ShouldReturnCorrectCategories()
        {
            IEnumerable<HouseCategoryServiceModel> result = houseService.AllCategories();
            DbSet<Category> dbCategoryies = data.Categories;

            Assert.AreEqual(dbCategoryies.Count(), result.Count());

            List<string> categoryNames = dbCategoryies.Select(c => c.Name).ToList();

            Assert.That(categoryNames.Contains(result.FirstOrDefault().Name));
        }

        [Test]
        public void HouseDetailsById_ShouldReturnCorrectHouseData()
        {
            int houseId = RentedHouse.Id;
            HouseDetailsServiceModel result = houseService.HouseDetailsById(houseId);

            Assert.IsNotNull(result);

            House houseInDb = data.Houses.Find(houseId);

            Assert.AreEqual(houseInDb.Id, result.Id);
            Assert.AreEqual(houseInDb.Title, result.Title);
        }

        [Test]
        public void Exists_ShouldReturnCorrectTrue_WithValidId()
        {
            int houseId = RentedHouse.Id;
            bool result = houseService.Exists(houseId);

            Assert.IsTrue(result);
        }

        [Test]
        public void AllHousesByUserId_ShouldReturnCorrectHouses()
        {
            string renterId = Renter.Id;
            IEnumerable<HouseServiceModel> result = houseService.AllHousesByUserId(renterId);

            Assert.IsNotNull(result);

            IEnumerable<House> housesInDb = data.Houses
                .Where(h => h.RenterId == renterId);

            Assert.AreEqual(housesInDb.Count(), result.Count());
        }

        [Test]
        public void AllHousesByAgentId_ShouldReturnCorrectHouses()
        {
            int agentId = Agent.Id;
            IEnumerable<HouseServiceModel> result = houseService.AllHousesByAgentId(agentId);

            Assert.IsNotNull(result);

            IEnumerable<House> housesInDb = data.Houses
                .Where(h => h.AgentId == agentId);

            Assert.AreEqual(housesInDb.Count(), result.Count());
        }

        [Test]
        public void AllCategoryNames_ShouldReturnCorrectResult()
        {
            IEnumerable<string> result = houseService.AllCategoriesNames();

            DbSet<Category> dbCategoryies = data.Categories;

            Assert.AreEqual(dbCategoryies.Count(), result.Count());

            IEnumerable<string> categoryNames = dbCategoryies.Select(c => c.Name);

            Assert.That(categoryNames.Contains(result.FirstOrDefault()));
        }

        [Test]
        public void All_ShouldReturnCorrectHouses()
        {
            string serchTerm = "First";

            HouseQueryServiceModel result = houseService.All(null, serchTerm);
            Assert.IsNotNull(result);

            IEnumerable<House> housesInDb = data.Houses
                .Where(h => h.Title.Contains(serchTerm));

            Assert.AreEqual(housesInDb.Count(), result.TotalHousesCount);

            var resultHouse = result.Houses.FirstOrDefault();
            House houseInDb = housesInDb.FirstOrDefault();

            Assert.AreEqual(houseInDb.Id, resultHouse.Id);
            Assert.AreEqual(houseInDb.Title, resultHouse.Title);
        }
    }
}