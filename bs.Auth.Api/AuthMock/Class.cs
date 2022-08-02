using bs.Auth.Interfaces.Dtos;
using bs.Auth.Interfaces.Models;
using bs.Auth.Interfaces.ViewModels;
using bs.Auth.Services;

namespace bs.Auth.Api.AuthMock
{
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
                Roles = user.Roles.Select(r => r.Code).ToArray()
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
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
