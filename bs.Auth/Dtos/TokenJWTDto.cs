using bs.Auth.Interfaces.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Auth.Dtos
{
    internal class TokenJWTDto : ITokenJWTDto
    {
        public TokenJWTDto(string token, DateTime expireAt)
        {
            Token = token;
            ExpireAt = expireAt;
        }

        public string Token {get;set;}
        public DateTime ExpireAt { get; set; }
    }
}
