using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Auth.Interfaces.ViewModels
{
    public interface IUserViewModel<T>
    {
        T Id { get; set; }
        string Username { get; set; }
        string Token { get; set; }
        DateTime TokenExpireAt { get; set; }

    }
}
