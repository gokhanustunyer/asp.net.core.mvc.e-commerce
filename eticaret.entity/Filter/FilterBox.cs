using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Filter
{
    public class FilterBox: BaseEntity.BaseEntity
    {
        public string FilterBoxTitle { get; set; }
        public ICollection<Filter> Filters { get; set; }
        public ICollection<Category.Category> Categories { get; set; }
    }
}
