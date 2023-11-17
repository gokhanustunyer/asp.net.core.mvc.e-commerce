using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.entity.Log
{
    public class PageLog: BaseEntity.BaseEntity
    {
        public int NumberOfClicks { get; set; }
        public string IpAddress { get; set; }
        public string UserName { get; set; }
        public string VisitedUrl { get; set; }
        public Product.Product? Product { get; set; }
        public Category.Category? Category { get; set; }
    }
}
