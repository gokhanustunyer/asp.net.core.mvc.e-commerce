using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.Product;
using eticaret.entity.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Concrete.Service
{
    public class SizeService : ISizeService
    {
        private readonly ISizeRepository _sizeRepository;

        public SizeService(ISizeRepository sizeRepository)
        {
            _sizeRepository = sizeRepository;
        }

        public async Task<Size> GenerateSize(string sizeName)
        {
            Size newSize = new Size()
            {
                Name = sizeName,
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };
            await _sizeRepository.AddAsync(newSize);
            await _sizeRepository.SaveAsync();

            return newSize;
        }

        public Size GetByName(string sizeName)
            => _sizeRepository.Table.Include(s => s.ProductSizes)
                      .ThenInclude(s => s.Product)
                     .FirstOrDefault(s => s.Name == sizeName);
    }
}
