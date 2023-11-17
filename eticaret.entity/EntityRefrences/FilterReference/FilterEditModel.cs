using eticaret.entity.EntityRefrences.ProductReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.FilterReference
{
    public class FilterEditModel
    {
        public string Id { get; set; }
        public string FilterBoxTitle { get; set; }
        public List<Filter.Filter> Filters { get; set; }
        public List<ProductListModel> Products { get; set; }
        public List<string> CategoryIds { get; set; }
        public List<TopCategory> TopCategories { get; set; }
        public List<Category.Category> Categories { get; set; }
    }
}
