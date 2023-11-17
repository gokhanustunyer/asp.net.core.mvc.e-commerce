using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.CartReference
{
    public class CheckoutModel
    {
        public string NameOnCart { get; set; }
        public string CartNumber { get; set; }
        public string ValidThrough { get; set; }
        public string CvcCode { get; set; }
    }
}
