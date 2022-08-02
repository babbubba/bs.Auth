using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Auth.Interfaces.Dtos
{
    public interface ITokenJWTDto
    {
        string Token { get; set; }
        DateTime ExpireAt { get; set; } 
    }
}
