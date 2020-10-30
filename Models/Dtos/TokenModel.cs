using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Dtos
{
    public class TokenModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
