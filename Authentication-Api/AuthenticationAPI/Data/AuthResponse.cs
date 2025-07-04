
using AuthenticationAPI.Data;
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
        private DateTime? lastPasswordChange;
        public string Password { get; set; }

        public DateTime? LastPasswordChange
        {
            get => lastPasswordChange;
            set => lastPasswordChange = value; // Set time part to 00:00:00
        }

        public AuthResponse(UserOrManager userDetail, string token)
        {
            Token = token;
            UserId = userDetail.userid;
            Username = userDetail.username;
            RoleId = userDetail.roleid;
            LastPasswordChange = userDetail.Last_Password_Change;
            Password = userDetail.password;
        }
    }
}
