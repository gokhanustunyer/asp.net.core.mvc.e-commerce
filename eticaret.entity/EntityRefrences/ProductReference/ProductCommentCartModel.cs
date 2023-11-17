using eticaret.entity.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.ProductReference
{
    public class ProductCommentCartModel
    {
        public ProductComment Comment { get; set; }
        public ProductListModel Product { get; set; }
    }
}
