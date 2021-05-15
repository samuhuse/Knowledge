using AutoMapper;

using SimpleApi.Model.Dtos;
using SimpleApi.Model.Dtos.Read;
using SimpleApi.Model.Dtos.Update;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Model.AutoMapperConfiguration
{
    public class CompanyMapping : Profile
    {
        public CompanyMapping()
        {
            CreateMap<Company, CompanyReadDto>().ReverseMap();
            CreateMap<Company, CompanyUpdateDto>().ReverseMap();
            CreateMap<Company, CompanyCreateDto>().ReverseMap();
        }
    }
}
