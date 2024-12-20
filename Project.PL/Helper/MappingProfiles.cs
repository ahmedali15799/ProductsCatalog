using AutoMapper;
using Project.DAL.Models;
using Project.PL.ViewModels;

namespace Project.PL.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();




        }
    }
}
