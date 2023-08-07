using AutoMapper;
using HouseRentingSystem.Services.Agents.Models;
using HouseRentingSystem.Services.Data.Entities;
using HouseRentingSystem.Services.Houses.Models;
using HouseRentingSystem.Services.Users.Models;

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

            CreateMap<Agent, UserServiceModel>()
                .ForMember(us => us.Email, cfg => cfg.MapFrom(ag => ag.User.Email))
                .ForMember(us => us.FullName, cfg => cfg.MapFrom(ag => ag.User.FirstName + " " + ag.User.LastName));

            CreateMap<User, UserServiceModel>()
                .ForMember(us => us.PhoneNumber, cfg => cfg.MapFrom(us => string.Empty))
                .ForMember(us => us.FullName, cfg => cfg.MapFrom(ag => ag.FirstName + " " + ag.LastName));

            CreateMap<Category, HouseCategoryServiceModel>();
        }
    }
}