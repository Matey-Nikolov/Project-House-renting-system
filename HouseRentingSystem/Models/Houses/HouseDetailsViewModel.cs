using HouseRentingSystem.Models.Agents;

namespace HouseRentingSystem.Models.Houses
{
    public class HouseDetailsViewModel : HouseViewModel
    {
        public string Title { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }

        public string Description { get; set; }
        public string Category { get; set; }

        public AgentViewModel Agent { get; set; }
    }
}