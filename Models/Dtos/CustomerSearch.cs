using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Dtos
{
    public class CustomerSearch
    {
        public string field { get; set; }
        public string value { get; set; }
        public string criteria { get; set; }
    }
}
