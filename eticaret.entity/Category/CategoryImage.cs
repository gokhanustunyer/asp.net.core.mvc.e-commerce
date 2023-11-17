using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Category
{
    public class CategoryImage : File
    {
        public ICollection<Category> Category { get; set; }
    }
}
