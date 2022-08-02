using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Auth.Interfaces.Models
{
    public interface IAppSecuritySettingsModel
    {
        int? JwtRefreshTokenValidityDays { get; set; }
        int? JwtTokenValidityMinutes { get; set; }
        string Secret { get; set; }
        string ValidIssuer { get; set; }
        string ValidAudience { get; set; }
        bool ValidateIssuer { get; set; }
        bool ValidateAudience { get; set; }
    }
}
