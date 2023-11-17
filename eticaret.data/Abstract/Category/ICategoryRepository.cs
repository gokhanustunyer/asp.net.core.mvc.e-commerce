using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity.Category;

namespace eticaret.data.Abstract.Category
{
    public interface ICategoryRepository: IRepository<et.Category>
    {

    }
}
