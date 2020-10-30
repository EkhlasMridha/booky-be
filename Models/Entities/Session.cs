using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class Session
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshExpiary { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
