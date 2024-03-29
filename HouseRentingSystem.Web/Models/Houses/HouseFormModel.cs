﻿using HouseRentingSystem.Services.Houses.Models;
using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Services.Data.DataConstants.House;

namespace HouseRentingSystem.Web.Models.Houses
{
    public class HouseFormModel : IHouseModel
    {
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; init; }

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        public string Address { get; init; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; init; }

        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; init; }

        [Required]
        [Range(0.00, MaxPricePerMonth, ErrorMessage = "Price per month must be a positive number and less than {2} leva.")]
        [Display(Name = "Price per mounth")]
        public decimal PricePerMonth { get; init; }


        public IEnumerable<HouseCategoryServiceModel> Categories { get; set; }
        = new List<HouseCategoryServiceModel>();
    }
}