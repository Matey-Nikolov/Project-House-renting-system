﻿using System.ComponentModel;

namespace HouseRentingSystem.Web.Models.Houses
{
    public class HouseViewModel
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Address { get; init; }

        [DisplayName("Image URL")]
        public string ImageUrl { get; init; }

        [DisplayName("Price Per Month")]
        public decimal PricePerMonth { get; init; }

        [DisplayName("Is Rented")]
        public bool IsRented { get; init; }
    }
}