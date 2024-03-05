using eticaret.data.Abstract.Audience;
using eticaret.data.Abstract.Category;
using eticaret.data.Contexts;
using eticaret.entity.Audience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Concrete.Audience
{
    public class AudienceRepository : Repository<UserSegment>, IAudienceRepository
    {
        public AudienceRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
