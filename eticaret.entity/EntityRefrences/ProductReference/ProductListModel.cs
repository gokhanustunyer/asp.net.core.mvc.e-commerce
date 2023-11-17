using eticaret.entity.Product;

namespace eticaret.entity.EntityRefrences.ProductReference
{
    public class ProductListModel
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double? DiscountedPrice { get; set; }
        public int? Stock { get; set; }
        public string? ImageUrl { get; set; }
        public string? MainImageUrl { get; set; }
        public string Url { get; set; }
        public string TopCategoryUrl { get; set; }
        public List<ProductSize>? Sizes { get; set; }
        public string? SelectedSizeName { get; set; }
        public double? SelectedSizePrice { get; set; }
        public int? Quantity { get; set; }
        public string? OrderId { get; set; }
        public int? Rate { get; set; }
    }
}
