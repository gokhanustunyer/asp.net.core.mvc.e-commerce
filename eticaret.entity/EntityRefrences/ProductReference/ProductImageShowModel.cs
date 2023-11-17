using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.ProductReference
{
    public class ProductImageShowModel
    {
        public string? Id { get; set; }
        public string Path { get; set; }
        public string ProductId { get; set; }
        public string? AltTag { get; set; }
        public int? Index { get; set; }
    }
}
