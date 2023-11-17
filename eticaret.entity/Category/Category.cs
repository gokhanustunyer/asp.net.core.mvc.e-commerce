using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bs = eticaret.entity.BaseEntity;
using et = eticaret.entity;

namespace eticaret.entity.Category
{
    public class Category: bs.BaseEntity
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? TopCategoryId { get; set; }
        public CategoryImage? CategoryImage { get; set; }
        public ICollection<et.Product.Product>? Products { get; set; }
        public ICollection<Filter.FilterBox>? FilterBoxes { get; set; }
    }
}
