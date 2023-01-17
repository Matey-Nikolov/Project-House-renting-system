using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace HouseRentingSystem.Models.Houses
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
        public string IsRented { get; init; }
    }
}