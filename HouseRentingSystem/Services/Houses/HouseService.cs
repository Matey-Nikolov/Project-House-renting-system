﻿using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Entities;
using HouseRentingSystem.Models;
using HouseRentingSystem.Services.Houses.Models;
using Microsoft.Build.Evaluation;

namespace HouseRentingSystem.Services.Houses
{
    public class HouseService : IHouseService
    {
        private readonly HouseRentingDbContext data;

        public HouseService(HouseRentingDbContext data)
            => this.data = data;

        public HouseQueryServiceModel All
            (
                string category = null,
                string searchTerm = null,
                HouseSorting sorting = HouseSorting.Newest,
                int currentPage = 1,
                int housesPerPage = 1
            )
        {
            var housesQuery = data.Houses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                housesQuery = data.Houses
                    .Where(h => h.Category.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                housesQuery = housesQuery
                    .Where(h =>
                    h.Title.ToLower().Contains(searchTerm.ToLower()) ||
                    h.Address.ToLower().Contains(searchTerm.ToLower()) ||
                    h.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            housesQuery = sorting switch
            {
                HouseSorting.Price => housesQuery
                .OrderBy(h => h.PricePerMonth),

                HouseSorting.NotRentedFirst => housesQuery
                .OrderBy(h => h.RenterId != null)
                .ThenByDescending(h => h.Id),
                _ => housesQuery.OrderByDescending(h => h.Id)
            };

            var houses = housesQuery
                .Skip((currentPage - 1) * housesPerPage)
                .Take(housesPerPage)
                .Select(h => new HouseServiceModel
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth
                })
                .ToList();

            var totalHouses = housesQuery.Count();

            return new HouseQueryServiceModel()
            {
                TotalHousesCount = totalHouses,
                Houses = houses
            };
        }

        public IEnumerable<string> AllCategoriesNames()
            => data.Categories
            .Select(c => c.Name)
            .Distinct()
            .ToList();

        public IEnumerable<HouseServiceModel> AllHousesByAgentId (int agentId)
        {
            List<House> houses = data
                .Houses
                .Where(h => h.AgentId == agentId)
                .ToList();

            return ProjectToModel(houses);
        }

        public IEnumerable<HouseServiceModel> AllHousesByUserId(string userId)
        {
            List<House> houses = data
                .Houses
                .Where(h => h.RenterId == userId)
                .ToList();

            return ProjectToModel(houses);
        }


        private List<HouseServiceModel> ProjectToModel(List<House> houses)
        {
            List<HouseServiceModel> resultHouses = houses
                .Select(h => new HouseServiceModel()
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId == null
                })
                .ToList();

            return resultHouses;
        }
    }
}