using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Offer
{
    public class Campaign: BaseEntity.BaseEntity
    {
        public string Title { get; set; }
        public  int DiscountRate { get; set; }
        public List<Category.Category> Categories { get; set; }
        public List<Product.Product> Products { get; set; }
        public  DateTime CodeStartDate { get; set; }
        public  DateTime CodeEndDate { get; set; }
    }
}
