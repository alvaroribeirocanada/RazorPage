using AutoMapper;
using HealthEquity.API.Entities;
using HealthEquity.API.Models;

namespace HealthEquity.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CarModel, Car>().ReverseMap();
        }
    }
}
