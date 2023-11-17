using eticaret.data.Abstract.Category;
using eticaret.data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity.Category;

namespace eticaret.data.Concrete.Category
{
    public class CategoryRepository : Repository<et.Category>, ICategoryRepository
    {
        public CategoryRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
