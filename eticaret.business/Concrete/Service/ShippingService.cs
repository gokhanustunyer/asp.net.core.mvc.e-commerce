using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.Shipping;
using eticaret.entity.Cargo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Concrete.Service
{
    public class ShippingService: IShippingService
    {
        private readonly IShippingRepository _shippingRepository;
        private readonly IConfiguration _configuration;

        public ShippingService(IShippingRepository shippingRepository, IConfiguration configuration)
        {
            _shippingRepository = shippingRepository;
            _configuration = configuration;
        }

        public async Task<bool> AddNewCompany(Shipping model)
        {
            if (model.IsDefault)
            {
                Shipping defaultCompany = await _shippingRepository.Table.FirstOrDefaultAsync(sc => sc.IsDefault);
                if (defaultCompany != null)
                {
                    defaultCompany.IsDefault = false;
                    _shippingRepository.Update(defaultCompany);
                }
            }
            model.Id = Guid.NewGuid();
            model.UpdateDate = DateTime.Now;
            model.CreateDate = DateTime.Now;
            bool result = await _shippingRepository.AddAsync(model);
            await _shippingRepository.SaveAsync();
            return result;
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            Shipping company = await _shippingRepository.GetByIdAsync(id);
            _shippingRepository.Remove(company);
            await _shippingRepository.SaveAsync();
            if (company.IsDefault)
            {
                Shipping newDefault = await _shippingRepository.Table.FirstOrDefaultAsync();
                newDefault.IsDefault = true;
                _shippingRepository.Update(newDefault);
            }
            await _shippingRepository.SaveAsync();
            return true;
        }

        public async Task<List<Shipping>> GetAllWithImages()
            => await _shippingRepository.Table.Include(s => s.ShippingImages).ToListAsync();

        public async Task<Shipping> GetCompanyForEdit(string id)
            => await _shippingRepository.Table.Include(s => s.ShippingImages).FirstOrDefaultAsync(s => s.Id.ToString() == id);

        public async Task<Shipping> GetDefaultAsync()
        {
            return await _shippingRepository.Table.FirstOrDefaultAsync(sc => sc.IsDefault);
        }
    }
}
