using System.ComponentModel.DataAnnotations;

namespace HouseRentingSystem.Data.Entities
{
    public class House
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public decimal PricePerMonth { get; set; }

        public int CategoryId { get; set; }
        //public Category Category { get; set; }

        public int AgenId { get; set; }
        //public Agent Agent { get; set; }

        public string RenterId { get; set; }
    }
}