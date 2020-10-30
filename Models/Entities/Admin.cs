using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class Admin
    {
        public Admin()
        {
            Sessions = new HashSet<Session>();
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string BexioToken { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
