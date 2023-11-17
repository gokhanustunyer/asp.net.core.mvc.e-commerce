using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Cargo
{
    public class ShippingImage: File
    {
        public List<Shipping>? Shippings { get; set; }
    }
}
