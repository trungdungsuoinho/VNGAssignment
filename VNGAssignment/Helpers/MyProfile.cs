using AutoMapper;
using VNGAssignment.Entities;
using VNGAssignment.Models;

namespace VNGAssignment.Helpers
{
    public class MyProfile : Profile
    {
        public MyProfile()
        {
            CreateMap<Book, AddBookRequest>().ReverseMap();
            CreateMap<Book, UpdateBookRequest>().ReverseMap();
        }
    }
}
