﻿using HouseRentingSystem.Services.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using static HouseRentingSystem.Services.Data.Constants;

namespace HouseRentingSystem.Services.Data
{
    public class HouseRentingDbContext : IdentityDbContext<User>
    {
        private User AgentUser { get; set; }
        private User GuestUser { get; set; }
        private User AdminUser { get; set; }

        private Agent Agent { get; set; }
        private Agent AdminAgent { get; set; }

        private Category CottageCategory { get; set; }
        private Category SingleCategory { get; set; }
        private Category DuplexCategory { get; set; }

        private House FirstHouse { get; set; }
        private House SecondHouse { get; set; }
        private House ThirdHouse { get; set; }


        public HouseRentingDbContext(DbContextOptions<HouseRentingDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<House> Houses { get; init; }
        public DbSet<Category> Categories { get; init; }
        public DbSet<Agent> Agents { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<House>()
                .HasOne(h => h.Category)
                .WithMany(c => c.Houses)
                .HasForeignKey(h => h.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<House>()
                .HasOne(h => h.Agent)
                .WithMany()
                .HasForeignKey(h => h.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            SeedUsers();
            builder.Entity<User>()
                .HasData(AgentUser, GuestUser, AdminUser);

            SeedAgent();
            builder.Entity<Agent>()
                .HasData(Agent, AdminAgent);

            SeedCategories();
            builder.Entity<Category>()
                .HasData(CottageCategory, SingleCategory, DuplexCategory);

            SeedHouses();
            builder.Entity<House>()
                .HasData(FirstHouse, SecondHouse, ThirdHouse);


            base.OnModelCreating(builder);
        }

        private void SeedUsers()
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();

            AdminUser = new User()
            {
                Id = "bcb4f072-ecca-43c9-ab26-c060c6f364e4",
                Email = AdminEmail,
                NormalizedEmail = AdminEmail,
                UserName = AdminEmail,
                NormalizedUserName = AdminEmail,
                FirstName = "Great",
                LastName = "Admin"
            };

            AdminUser.PasswordHash =
                hasher.HashPassword(AgentUser, "admin123");

            AgentUser = new User()
            {
                Id = "deal2856-c198-4129-b3f3-b893d8395082",
                UserName = "agent@mail.com",
                FirstName = "Linda",
                LastName = "Michaels",
                NormalizedUserName = "agent@mail.com",
                Email = "agent@mail.com",
                NormalizedEmail = "agent@mail.com"
            };

            AgentUser.PasswordHash =
            hasher.HashPassword(AgentUser, "agent123");

            GuestUser = new User()
            {
                Id = "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                UserName = "guest@mail.com",
                FirstName = "Teodor",
                LastName = "Lesly",
                NormalizedUserName = "guest@mail.com",
                Email = "guest@mail.com",
                NormalizedEmail = "guest@mail.com"
            };

            GuestUser.PasswordHash =
            hasher.HashPassword(AgentUser, "guest123");
        }

        private void SeedAgent()
        {
            AdminAgent = new Agent()
            {
                Id = 5,
                PhoneNumber = "+359123456789",
                UserId = AdminUser.Id
            };

            Agent = new Agent()
            {
                Id = 1,
                PhoneNumber = "+359888888888",
                UserId = AgentUser.Id
            };
        }

        private void SeedCategories()
        {
            CottageCategory = new Category()
            {
                Id = 1,
                Name = "Cottage"
            };

            SingleCategory = new Category()
            {
                Id = 2,
                Name = "Single-Family"
            };

            DuplexCategory = new Category()
            {
                Id = 3,
                Name = "Duplex"
            };
        }

        private void SeedHouses()
        {
            FirstHouse = new House()
            {
                Id = 1,
                Title = "Big House Marina",
                Address = "North London, UK (near the border)",
                Description = "A big house for your whole family. Don't miss to buy a house with three bedrooms.",
                ImageUrl = "https://www.luxury-architecture.net/wp-content/uploads/2017/12/1513217889-7597-FAIRWAYS - 010.jpg",
                PricePerMonth = 2100.00M,
                CategoryId = DuplexCategory.Id,
                AgentId = Agent.Id,
                RenterId = this.GuestUser.Id
            };
            
            SecondHouse = new House()
            {
                Id = 2,
                Title = "Family House Comfort",
                Address = "Near the Sea Garden in Burgas, Bulgaria",
                Description = "It has the best comfort you will ever ask for. With two bedrooms, it isgreat for your family.",
                ImageUrl = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg" + "?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1",
                PricePerMonth = 1200.00M,
                CategoryId = SingleCategory.Id,
                AgentId = Agent.Id
            };

            ThirdHouse = new House()
            {
                Id = 3,
                Title = "Grand House",
                Address = "Boyana Neighbourhood, Sofia, Bulgaria",
                Description = "This luxurious house is everything you will need. It is just excellent.",
                ImageUrl = "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg",
                PricePerMonth = 2000.00M,
                CategoryId = SingleCategory.Id,
                AgentId = Agent.Id
            };
        }
    }
}