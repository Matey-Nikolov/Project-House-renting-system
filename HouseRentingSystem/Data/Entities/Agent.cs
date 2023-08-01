using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Data.DataConstants.Agent;

namespace HouseRentingSystem.Data.Entities
{
    public class Agent
    {
        //[Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; init; }
    }
}