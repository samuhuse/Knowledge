using JwtBearerApiAuthorization.Config;
using JwtBearerApiAuthorization.Data;
using JwtBearerApiAuthorization.Model.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtBearerApiAuthorization.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public UserRepository(ApplicationDbContext context, JwtSettings jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings;
        }

        public User Authenticate(string username, string password)
        {
            User user = _context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
            
            if (user is null) return null;

            var token = _jwtSettings.TokenHandler.CreateToken(_jwtSettings.GetTokenDescriptor(user.Id.ToString(), user.Role));
            user.Token = _jwtSettings.TokenHandler.WriteToken(token);

            return user;
        }

        public bool IsUserunique(string username)
        {
            return !_context.Users.Any(u => u.Username == username);
        }

        public User Register(string username, string password)
        {
            if (!IsUserunique(username)) return null;

            _context.Users.Add(new User { Username = username, Password = password });
            _context.SaveChanges();

            return Authenticate(username, password);
        }
    }
}
