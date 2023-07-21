using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Entities;
using HouseRentingSystem.Services.Statistics.Models;

namespace HouseRentingSystem.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly HouseRentingDbContext data;

        public StatisticsService(HouseRentingDbContext data)
        {
            this.data = data;
        }

        public StatisticsServiceModel Total()
        {
            int totalHouses = data.Houses.Count();
            int totalRents = data.Houses.Where(h => h.RenterId != null).Count();

            return new StatisticsServiceModel
            {
                TotalHouses = totalHouses,
                TotalRents = totalRents
            };
        }
    }
}