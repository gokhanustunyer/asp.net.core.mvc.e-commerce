using eticaret.data.Abstract.Logs;
using eticaret.data.Contexts;
using eticaret.entity.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Concrete.Logs
{
    public class PageLogRepository : Repository<PageLog>, IPageLogRepository
    {
        public PageLogRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
