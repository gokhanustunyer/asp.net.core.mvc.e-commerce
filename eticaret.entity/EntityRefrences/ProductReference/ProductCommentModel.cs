using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.ProductReference
{
    public class ProductCommentModel
    {
        public string FullName { get; set; }
        public string Comment { get; set; }
        public int Rate { get; set; }
        public int? GeneralRate { get; set; }
        public string? UpdateDate { get; set; }
        public string ProductId { get; set; }
        public List<string> ImagePaths { get; set; }
        public ProductListModel? Product { get; set; }
    }
}
