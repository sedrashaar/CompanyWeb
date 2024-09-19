using AutoMapper;
using Company.Data.Entities;
using Company.Services.Interfaces.Department.Dto;

namespace Company.Services.Mapping
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department,DepartmentDto>().ReverseMap();
        }
  
    }
}
