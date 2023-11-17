using eticaret.entity.EntityRefrences.CategoryReference;
using System.ComponentModel.DataAnnotations;

namespace eticaret.entity.EntityRefrences.ProductReference
{
    public class ProductCreateModel
    {
        public string? Name { get; set; }

        public string? Description { get; set; }
        public string? ShortDescription { get; set; }

        public double Price { get; set; }
        
        public int Stock { get; set; }
        public string? RelatedProductIds { get; set; }

        public List<TopCategory> TopCategories { get; set; }
        
        public List<CategoryListModel> Categories { get; set; }
    }
}
