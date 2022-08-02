using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Auth.Interfaces.Models
{
    public interface IUserModel<T>
    {
        T Id { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        IRoleModel<T>[] Roles { get; set; }
    }
}
