using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.data.Abstract.Filter
{
    public interface IFilterRepository: IRepository<et.Filter.Filter>
    {
    }
}
