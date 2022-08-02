using bs.Auth.Interfaces.Dtos;
using bs.Auth.Interfaces.Models;
using bs.Auth.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Auth.Interfaces.Services
{
    public  interface IAuthService
    {
        Task<IUserViewModel<T>> AuthenticateAsync<T>(IUserAuthDto userAuth);
        ITokenJWTDto GenerateClaimsAndToken<T>(IUserModel<T> user);
    }
}
