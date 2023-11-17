using eticaret.entity.EntityRefrences.CartReference;
using eticaret.entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.UserReference
{
    public class UserCheckOutModel
    {
        public UserCartModel CartModel { get; set; }
        public AppUser CartUser { get; set; }
    }
}
