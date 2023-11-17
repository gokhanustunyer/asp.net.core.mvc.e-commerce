using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.ProductReference
{
    public class ProductQAListModel
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string FullName { get; set; }
        public DateTime PublishDate { get; set; }
        public List<ProductQAListModel>? Responses { get; set; }
    }
}
