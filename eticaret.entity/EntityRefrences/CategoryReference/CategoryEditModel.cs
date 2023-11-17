using eticaret.entity.EntityRefrences.ProductReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity.Category;

namespace eticaret.entity.EntityRefrences.CategoryReference
{
    public class CategoryEditModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TopCategoryId { get; set; }
        public string? Url { get; set; }
        public string? ImagePath { get; set; }
        public List<et.Category> SubCategories { get; set; }
        public List<TopCategory> AllCategories { get; set; }
        public List<et.Category> TopCategories { get; set; }
    }
}
