using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthIds.server.Models
{
    public class User
    {
        public long UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Login { get; set; }

        public string PasseWord { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
        public bool IsActive { get; internal set; }
    }
}
