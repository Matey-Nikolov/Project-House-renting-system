using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Data.DataConstants.User;

namespace HouseRentingSystem.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(UserFirstNameMaxLenght)]
        public string FirstName { get; init; }

        [Required]
        [MaxLength(UserLastNameMaxLenght)]
        public string LastName { get; init; }
    }
}