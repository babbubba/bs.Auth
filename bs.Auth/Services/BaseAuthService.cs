using bs.Auth.Dtos;
using bs.Auth.Interfaces.Dtos;
using bs.Auth.Interfaces.Models;
using bs.Auth.Interfaces.Services;
using bs.Auth.Interfaces.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace bs.Auth.Services
{
    public abstract class BaseAuthService : IAuthService
    {
        private readonly IAppSecuritySettingsModel securitySettings;

        public BaseAuthService(IAppSecuritySettingsModel securitySettings)
        {
            this.securitySettings = securitySettings;
        }

        /// <summary>
        /// Validate the username and password then obtain the UserModel and generate Claims and Tokens
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userAuth"></param>
        /// <returns></returns>
        public abstract Task<IUserViewModel<T>> AuthenticateAsync<T>(IUserAuthDto userAuth);

        /// <summary>
        /// Generate Claims and JWT Token from a valid User model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ITokenJWTDto GenerateClaimsAndToken<T>(IUserModel<T> user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user), "Cannot generate Claims and Token with undefined User!");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("userName", user.Username),
                new Claim("userId", user.Id.ToString()),
            };

            claims.AddRange(user.Roles.Select(g => new Claim("role", g.Code)));

            return GenerateJwtToken(claims);
        }

        /// <summary>
        /// Generate a JWT Token base on the provided claims
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        protected ITokenJWTDto GenerateJwtToken(IList<Claim> claims)
        {
            var expireAt = DateTime.UtcNow.AddMinutes(securitySettings.JwtTokenValidityMinutes ?? 15);

            // Add JWT Id Claim
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            // generate token 
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(securitySettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = securitySettings.ValidIssuer,
                Audience = securitySettings.ValidAudience,
                Expires = expireAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new TokenJWTDto(tokenHandler.WriteToken(token), expireAt);
        }
    }
}
