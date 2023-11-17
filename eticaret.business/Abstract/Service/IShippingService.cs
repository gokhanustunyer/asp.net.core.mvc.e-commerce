using eticaret.entity.Cargo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Service
{
    public interface IShippingService
    {
        Task<List<Shipping>> GetAllWithImages();
        Task<Shipping> GetCompanyForEdit(string id);
        Task<bool> AddNewCompany(Shipping model);
        Task<Shipping> GetDefaultAsync();
        Task<bool> DeleteByIdAsync(string id);
    }
}
