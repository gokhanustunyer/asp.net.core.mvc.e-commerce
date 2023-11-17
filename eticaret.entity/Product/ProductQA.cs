using eticaret.entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Product
{
    public class ProductQA: BaseEntity.BaseEntity
    {
        public string Message { get; set; }
        public bool isConfirmed { get; set; }
        public Product Product { get; set; }
        public AppUser User { get; set; }
        public ProductQA? UpperQA { get; set; }
    }
}
