using eticaret.entity.EntityRefrences.ProductReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.Discount
{
    public class EditCampaignModel
    {
        public string Code { get; set; }
        public string Id { get; set; }
        public DateTime CodeStartDate { get; set; }
        public DateTime CodeEndDate { get; set; }
        public int CodeLimitNumber { get; set; }
        public int DiscountRate { get; set; }
        public double DiscountNumber { get; set; }
        public List<Category.Category> Categories { get; set; }
        public List<ProductListModel> Products { get; set; }
        public List<TopCategory> TopCategories { get; set; }
    }
}
