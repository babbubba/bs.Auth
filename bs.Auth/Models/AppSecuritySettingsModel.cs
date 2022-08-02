using bs.Auth.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Auth.Models
{
    public class AppSecuritySettingsModel : IAppSecuritySettingsModel
    {
        public int? JwtRefreshTokenValidityDays { get; set; }
        public int? JwtTokenValidityMinutes { get; set; }
        public string Secret { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
    }
}
