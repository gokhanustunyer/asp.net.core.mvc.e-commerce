using eticaret.entity.EntityRefrences.ProductReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.FilterReference
{
    public class FilterDetailsModel
    {
        public string FilterId { get; set; }
        public string FilterTitle { get; set; }
        public List<ProductListModel>? Products { get; set; }
    }
}
