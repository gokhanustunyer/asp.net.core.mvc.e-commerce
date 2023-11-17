using eticaret.data.Abstract.Category;
using eticaret.data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.data.Concrete.Category
{
    public class SubCategoryRepository : Repository<et.Category.SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(ETicaretDbContext context) : base(context)
        {
        }
    }
}
