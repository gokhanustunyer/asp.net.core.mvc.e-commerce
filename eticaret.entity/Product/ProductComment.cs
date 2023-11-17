using eticaret.entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.entity.Product
{
    public class ProductComment: et.BaseEntity.BaseEntity
    {
        public string Comment { get; set; }
        public bool isHavePhoto { get; set; }
        public bool isConfirmed { get; set; }
        public AppUser User { get; set; }
        public ProductRate Rate { get; set; }
        public Product Product { get; set; }
        public List<CommentImage>? Images { get; set; }
    }
}
