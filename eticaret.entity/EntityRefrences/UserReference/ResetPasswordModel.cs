using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.UserReference
{
    public class ResetPasswordModel
    {
        public string Token { get; set; }
        public string userId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
    }
}
