using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.data.Abstract.Shipping
{
    public interface IShippingRepository: IRepository<et.Cargo.Shipping>
    {

    }
}
