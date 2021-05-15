using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using JwtBearerApiAuthorization.Model.Authorization;
using JwtBearerApiAuthorization.Model.Authorization.Dtos;
using JwtBearerApiAuthorization.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtBearerApiAuthorization.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] Credential credential)
        {
            User user = _userRepository.Authenticate(credential.Username, credential.Password);

            if (user is null) return BadRequest(new { message = "Username or Password incorrect" });
            return Ok(_mapper.Map<UserReadDto>(user));
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] Credential credential)
        {
            if (!_userRepository.IsUserunique(credential.Username)) return BadRequest(new { message = "Username already exists"});

            User user = _userRepository.Register(credential.Username, credential.Password);
            if (user is null) return BadRequest(new { message = "Error while registering the user" });

            return Ok(_mapper.Map<UserReadDto>(user));
        }

    }
}
