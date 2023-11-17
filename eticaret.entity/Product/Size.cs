using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.entity.Product
{
    public class Size: et.BaseEntity.BaseEntity
    {
        public string Name { get; set; }
        public ICollection<ProductSize> ProductSizes { get; set; }

    }
}
