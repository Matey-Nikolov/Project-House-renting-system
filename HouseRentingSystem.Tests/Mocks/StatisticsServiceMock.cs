﻿using HouseRentingSystem.Services.Statistics;
using HouseRentingSystem.Services.Statistics.Models;


namespace HouseRentingSystem.Tests.Mocks
{
    public class StatisticsServiceMock
    {
        public static IStatisticsService Instance
        {
            get
            {
                Mock<IStatisticsService> statisticsServiceMock = new Mock<IStatisticsService>();

                statisticsServiceMock
                    .Setup(s => s.Total())
                    .Returns(new StatisticsServiceModel()
                    {
                        TotalHouses = 10,
                        TotalRents = 6,
                    });

                return statisticsServiceMock.Object;
            }
        }
    }
}