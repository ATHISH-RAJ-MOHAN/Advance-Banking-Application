
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClassLibrary
{
    public class AuthResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public int RoleId { get; set; }

        public AuthResponse(UserOrManager userDetail, string token)
        {
            Token = token;
            UserId = userDetail.userid;
            Username = userDetail.username;
            RoleId = userDetail.roleid;
        }
    }
}
