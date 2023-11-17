using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Product
{
    public class CommentImage: File
    {
        public List<ProductComment> ProductComment { get; set; }
    }
}
