using System;
using System.Collections.Generic;
using System.Text;

namespace AuthorizationService.CorsConfig
{
    class CorsHosts : ICorsHosts
    {
        public string AllowedHost { get; set; }
    }

    public interface ICorsHosts
    {
        public string AllowedHost { get; set; }
    }
}
