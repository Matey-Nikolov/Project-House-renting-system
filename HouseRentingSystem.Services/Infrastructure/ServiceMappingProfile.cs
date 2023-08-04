using AutoMapper;
using HouseRentingSystem.Services.Agents.Models;
using HouseRentingSystem.Services.Data.Entities;
using HouseRentingSystem.Services.Houses.Models;

namespace HouseRentingSystem.Services.Infrastructure
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile() 
        {
            CreateMap<House, HouseIndexServiceModel>();

            CreateMap<House, HouseServiceModel>()
                .ForMember(h => h.IsRented, cfg => cfg.MapFrom(h => h.RenterId != null));

            CreateMap<House, HouseDetailsServiceModel>()
                 .ForMember(h => h.IsRented, cfg => cfg.MapFrom(h => h.RenterId != null))
                 .ForMember(h => h.Category, cfg => cfg.MapFrom(h => h.Category.Name));

            CreateMap<Agent, AgentServiceModel>()
                .ForMember(ag => ag.Email, cfg => cfg.MapFrom(ag => ag.User.Email));

            CreateMap<Category, HouseCategoryServiceModel>();
        }
    }
}