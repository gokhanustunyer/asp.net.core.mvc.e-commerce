using eticaret.entity.EntityRefrences.CategoryReference;
using eticaret.entity.EntityRefrences.ProductReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.ProductCategoryReference
{
    public class ProductCategoryDetailModel
    {
        public ProductListModel Product { get; set; }
        public List<CategoryListModel> Categories { get; set; }
    }
}
