using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICore.Models
{
    public class UserDetails
    {
        public Users user { get; set; }
        public UserImage UserImg { get; set; }
    }
    public class Users
    {
        public int uid { get; set; }
        public string fullname { get; set; }
        public string contact { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string status { get; set; } // active, inactive & deleted
        public string roles { get; set; } // admin, user & customer
        public string image { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
    }

    public class UserImage
    {
        public int imgid { get; set; }
        public int uid { get; set; }
        public string image { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime dateupdated { get; set; }
    }
}
