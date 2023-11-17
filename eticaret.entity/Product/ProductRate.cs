using eticaret.entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.entity.Product
{
    public class ProductRate: et.BaseEntity.BaseEntity
    {
        public AppUser User { get; set; }
        public Product Product { get; set; }
        public int Rate { get; set; }
    }
}
