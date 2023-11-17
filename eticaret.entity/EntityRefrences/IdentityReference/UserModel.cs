using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.IdentityReference
{
    public class UserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime CreateDate { get; set; }
        public string Email { get; set; }
        public bool isExternal { get; set; }
    }
}
