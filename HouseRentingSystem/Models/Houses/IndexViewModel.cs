namespace HouseRentingSystem.Models.Houses
{
    public class IndexViewModel
    {
        public IEnumerable<HouseDetailsViewModel> Houses { get; set; }
        = new List<HouseDetailsViewModel>();
    }
}