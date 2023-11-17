using eticaret.entity.Identity;
using eticaret.entity.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.entity.Product
{

    public class Product: et.BaseEntity.BaseEntity
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public bool isActive { get; set; }
        public double Price { get; set; }
        public int? Stock { get; set; }
        public string? MainImageId { get; set; }
        public AppUser Publisher { get; set; }
        public ICollection<Filter.Filter>? Filters { get; set; }
        public ICollection<ProductRate> Rates { get; set; }
        public ICollection<ProductComment> Comments { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductDescImage> ProductDescImages { get; set; }
        public ICollection<et.Category.Category> Categories { get; set; }
        public ICollection<ProductSize> ProductSizes { get; set; }
        public ICollection<RelatedProduct> RelatedProducts { get; set; }
        public List<DiscountCode> DiscountCodes { get; set; }
        public List<Campaign> Campaigns { get; set; }
        public List<ProductQA> QAs { get; set; }
    }


}
