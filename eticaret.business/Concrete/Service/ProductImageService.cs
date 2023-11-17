using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.File;
using eticaret.data.Abstract.Product;
using eticaret.entity.EntityRefrences.FileReference;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.business.Concrete.Service
{
    public class ProductImageService : IProductImageService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IConfiguration _configuration;
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;

        public ProductImageService(IFileRepository fileRepository,
                                   IConfiguration configuration,
                                   IProductRepository productRepository,
                                   IProductImageRepository productImageRepository)
        {
            _fileRepository = fileRepository;
            _configuration = configuration;
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
        }

        public async Task<List<ProductImageShowModel>> GetByProductIdAsync(string productId)
        {
            var product = await _productRepository.Table.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == Guid.Parse(productId));
            return product.ProductImages.Select(p => new ProductImageShowModel()
            {
                Path = _configuration[$"StoragePaths:{p.Storage}"] + p.Path,
                Id = p.Id.ToString(),
                Index = p.Index,
                ProductId = productId
            }).OrderBy(i => i.Index).ToList();
        }

        public string GetMainImagePathByProductId(string productId)
        {
            var product = _productRepository.Table
                            .FirstOrDefault(p => p.Id.ToString() == productId);
            string path = _productImageRepository.Table.FirstOrDefault
                    (pi => pi.Id.ToString() == product.MainImageId).Path;
            return _configuration["StoragePaths:Azure"] + path;
        }

        public async Task<bool> ReIndex(List<ImageAndIndexsModel> alignment, string productId)
        {
            if (alignment == null || productId == null)
            {
                return false;
            }
            foreach(ImageAndIndexsModel model in alignment)
            {
                ProductImage image = await _productImageRepository.Table.FirstOrDefaultAsync(i => i.Id.ToString() == model.imageId);
                image.Index = model.index;
                _productImageRepository.Update(image);
            }
            await _productImageRepository.SaveAsync();
            return true;
        }
    }
}
