using eticaret.entity.Product;
using et = eticaret.entity;

namespace eticaret.entity.EntityRefrences.ProductReference
{
    public class ProductEditModel
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public List<string> RelatedIds { get; set; }
        public List<et.Filter.Filter> Filters { get; set; }
        public List<ProductSize> Options { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<et.Category.Category> ActiveCategories { get; set; }
        public List<et.Category.Category> AllCategories { get; set; }
        public List<TopCategory> TopCategories { get; set; }
        public string Path { get; set; }
    }
}
