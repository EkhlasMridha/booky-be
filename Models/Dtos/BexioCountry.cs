using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Dtos
{
    public class BexioCountry
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name_short { get; set; }
        public string iso3166_alpha2 { get; set; }
    }
}
