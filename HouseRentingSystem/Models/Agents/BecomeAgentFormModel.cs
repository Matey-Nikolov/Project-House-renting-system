using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Data.DataConstants.Agent;

namespace HouseRentingSystem.Models.Agents
{
    public class BecomeAgentFormModel
    {
        [Required]
        [Phone]
        [Display(Name ="Phone Number")]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        public string PhoneNumber { get; init; }
    }
}