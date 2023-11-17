using eticaret.data.Abstract.Filter;
using eticaret.data.Contexts;
using eticaret.entity.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Concrete.Filter
{
    public class FilterBoxRepository : Repository<FilterBox>, IFilterBoxRepository
    {
        public FilterBoxRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
