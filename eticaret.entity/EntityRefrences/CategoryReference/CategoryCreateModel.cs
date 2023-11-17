using eticaret.entity.EntityRefrences.ProductReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.CategoryReference
{
    public class CategoryCreateModel
    {
        public List<TopCategory> TopCategories { get; set; }
        public List<CategoryListModel> Categories { get; set; }
    }
}
