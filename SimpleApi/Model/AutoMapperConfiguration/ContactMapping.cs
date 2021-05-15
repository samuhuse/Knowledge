using AutoMapper;

using SimpleApi.Model.Dtos;
using SimpleApi.Model.Dtos.Create;
using SimpleApi.Model.Dtos.Read;
using SimpleApi.Model.Dtos.Update;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Model.AutoMapperConfiguration
{
    public class ContactMapping : Profile
    {
        public ContactMapping()
        {
            CreateMap<Contact, ContactUpdateDto>().ReverseMap();                           
            CreateMap<Contact, ContactCreateDto>().ReverseMap();
            CreateMap<Contact, ContactReadDto>();
        }
    }
}
