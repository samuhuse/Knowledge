using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using JwtBearerApiAuthorization.Model.Authorization;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtBearerApiAuthorization.Config
{
    public class JwtSettings
    {
        private readonly AppSettings _appSettings;
        public readonly JwtSecurityTokenHandler TokenHandler;

        public JwtSettings(IOptions<AppSettings> appSettingsOptions)
        {
            _appSettings = appSettingsOptions.Value;
            TokenHandler = new JwtSecurityTokenHandler();
        }

        public SecurityTokenDescriptor GetTokenDescriptor(string name, string role) 
        {
            return
            new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Role, role ?? "" )
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret)),SecurityAlgorithms.HmacSha256Signature)
            }; 
        }
    }
}
