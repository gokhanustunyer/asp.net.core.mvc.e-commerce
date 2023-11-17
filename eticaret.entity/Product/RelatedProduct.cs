using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Product
{
    public class RelatedProduct
    {
        [Key]
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public string RelatedProductId { get; set; }
    }
}
