using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Services.Data.DataConstants.Agent;

namespace HouseRentingSystem.Web.Models.Agents
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