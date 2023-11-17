using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.data.Abstract.Product
{
    public interface IProductRepository: IRepository<et.Product.Product>
    {
        List<et.Product.Product> GetAllWithImages();
    }
}
