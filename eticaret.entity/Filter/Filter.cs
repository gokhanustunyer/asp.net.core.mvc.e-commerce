using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Filter
{
    public class Filter: BaseEntity.BaseEntity
    {
        public string FilterTitle { get; set; }
        public ICollection<Product.Product>? Products { get; set; }
    }
}
