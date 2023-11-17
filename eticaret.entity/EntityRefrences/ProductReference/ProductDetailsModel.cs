using eticaret.entity.EntityRefrences.CategoryReference;
using eticaret.entity.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.entity.EntityRefrences.ProductReference
{
    public class ProductDetailsModel
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int? DiscountRate { get; set; }
        public int? TotalStock { get; set; }
        public int Rate { get; set; }
        public int RateCount { get; set; }
        public string? QuestionedCategoryUrl { get; set; }
        public List<Category.Category> Categories { get; set; }
        public List<CategoryListModel> CategoriesByHierarchy { get; set; }
        public string MainImagePath { get; set; }
        public List<string> ImagePaths { get; set; }
        public List<OptionModel> Options { get; set; }
        public List<ProductListModel> SimilarProducts { get; set; }
        public List<ProductListModel> RelatedProducts { get; set; }
    }
}
