using Microsoft.VisualStudio.TestTools.UnitTesting;
using bs.Auth.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bs.Auth.Models;
using bs.Auth.Interfaces.ViewModels;
using bs.Auth.Interfaces.Dtos;
using bs.Auth.Interfaces.Models;

namespace bs.Auth.Services.Tests
{
    [TestClass()]
    public class BaseAuthServiceTests
    {
        [TestMethod()]
        public void AuthenticateAsyncTest()
        {
            // First set security model
            var security = new AppSecuritySettingsModel
            {
                JwtTokenValidityMinutes = 60,
                Secret = "123456789012345678901234567890123456",
                ValidateAudience = false,
                ValidateIssuer = false
            };
            var authService = new AuthService(security);

            var login = new UserAuthDto
            {
                Username = "Pippo",
                Password = "123"
            };

            var response = authService.AuthenticateAsync<int>(login).Result;
            Assert.IsNotNull(response);

        }
    }


    public class AuthService : BaseAuthService
    {
        public AuthService(IAppSecuritySettingsModel securitySettings) : base(securitySettings)
        {
        }

        public override async Task<IUserViewModel<T>> AuthenticateAsync<T>(IUserAuthDto userAuth)
        {
            // dummy autentication
            if (userAuth.Password != "123")
            {
                throw new ApplicationException("Invalid credential");
            }

            // instead of retriving user model from db we create a mock
            var user = new UserModel<T>
            {
                Id = default(T),
                Username = "Pinco",
                Password = "123",
                Roles = new List<IRoleModel<T>>
                {
                    new RoleModel<T>
                    {
                        Id = default(T),
                        Code = "admin",
                        IsActive = true,
                        Name = "Administrator"
                    }
                }.ToArray()
            };

            var token = GenerateClaimsAndToken(user);

            var userViewModel = new UserViewModel<T>
            {
                Id = user.Id,
                Username = user.Username,
                Token = token.Token,
                TokenExpireAt = token.ExpireAt,
                Roles = user.Roles.Select(r=>r.Code).ToArray()
            };

            return userViewModel;
        }
    }


    public class UserModel<T> : IUserModel<T>
    {
        public T Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public IRoleModel<T>[] Roles { get; set; }
    }

    public class RoleModel<T> : IRoleModel<T>
    {
        public T Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserViewModel<T> : IUserViewModel<T>
    {
        public T Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpireAt { get; set; }
        public string[] Roles { get; set; }

    }

    public class UserAuthDto : IUserAuthDto
    {
        public string Username  { get; set;}
        public string Password  { get; set;}
    }

}