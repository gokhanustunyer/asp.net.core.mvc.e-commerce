using eticaret.data.Abstract.PageStrings;
using eticaret.data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.data.Concrete.PageStrings
{
    public class PageStringsRepository : Repository<et.PageStrings.PageStrings>, IPageStringsRepository
    {
        public PageStringsRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
