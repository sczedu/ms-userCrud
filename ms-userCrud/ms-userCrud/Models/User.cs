using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_userCrud.Models
{
    public class User : UserId
    {
        public string Username { get; set; }
        public string Document { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
