using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClassLibrary
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string? username { get; set; }
        [Required(ErrorMessage = "Password required")]
        public string? password { get; set; }
    }
}
