using eticaret.data.Abstract.Filter;
using eticaret.data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.data.Concrete.Filter
{
    public class FilterRepository : Repository<et.Filter.Filter>, IFilterRepository
    {
        public FilterRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
