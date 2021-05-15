using AutoMapper;

using JwtBearerApiAuthorization.Model.Authorization.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtBearerApiAuthorization.Model.Authorization.AutoMapperConfig
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserReadDto>();
        }
    }
}
