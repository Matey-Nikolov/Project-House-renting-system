namespace HouseRentingSystem.Tests.UnitTests
{
    public class UnitTestsBase
    {
        protected HouseRentingDbContext data;

        public User Renter { get; private set; }
        public Agent Agent { get; private set; }
        public House RentedHouse { get; private set; }

        [OneTimeSetUp]
        public void SetUpBase()
        {
            data = DatabaseMock.Instance;
            SeedDatabase();
        }

        [OneTimeTearDown]
        public void TearDownBase()
        {
            data.Dispose();
        }

        private void SeedDatabase()
        {
            Renter = new User()
            {
                Id = "RenterUserId",
                Email = "rent@er.bg",
                FirstName = "Renter",
                LastName = "User"
            };

            data.Users.Add(Renter);

            Agent = new Agent()
            {
                PhoneNumber = "+359111111111",
                User = new User()
                {
                    Id = "TestUserId",
                    Email = "test@test.bg",
                    FirstName = "Test",
                    LastName = "Tester"
                }
            };

            data.Agents.Add(Agent);

            RentedHouse = new House()
            {
                Title = "First Test House",
                Address = "Add because is required",
                Description = "Add because is required",
                ImageUrl = "Add because is required",
                Renter = Renter,
                Agent = Agent,
                Category = new Category()
                {
                    Name = "Cottage"
                }
            };

            data.Houses.Add(RentedHouse);

            House nonRentedHouse = new House()
            {
                Title = "Second Test House",
                Address = "Add because is required",
                Description = "Add because is required",
                ImageUrl = "Add because is required",
            };

            data.Houses.Add(nonRentedHouse);
            data.SaveChanges();
        }
    }
}