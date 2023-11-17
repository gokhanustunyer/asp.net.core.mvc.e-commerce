using eticaret.entity.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Service
{
    public interface ISizeService
    {
        Size GetByName(string sizeName);

        Task<Size> GenerateSize(string sizeName);
    }
}
