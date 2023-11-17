
using eticaret.entity.EntityRefrences.CategoryReference;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Filter;
using eticaret.entity.Product;

namespace eticaret.entity.EntityRefrences.ProductCategoryReference
{
    public class ProductCategoryPageModel
    {
        public List<ProductListModel> Products { get; set; }
        public List<CategoryListModel> Categories { get; set; }
        public List<FilterBox> Filters { get; set; }
        public List<string> FilterIds { get; set; }
        public List<Category.Category> CategoryPath { get; set; }
    }
}
