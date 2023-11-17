using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.entity.Category
{
    public class SubCategory: et.BaseEntity.BaseEntity
    {
        public string? Name { get; set; }
        public ICollection<et.Product.Product>? Products { get; set; }
        public Category Category { get; set; }
    }
}
