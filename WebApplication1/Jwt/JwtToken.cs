using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Jwt
{
    public class JwtToken
    {
        public static async Task<string> GerarJwt(UserManager<ApplicationUser> _userManager, IOptions<AppSettings> _appSettings, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var identityClaims = new ClaimsIdentity();
            var claims = await _userManager.GetClaimsAsync(user);
            identityClaims.AddClaims(claims);

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
             
                }),
                Issuer = _appSettings.Value.Emissor,
                Audience = _appSettings.Value.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.Value.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}