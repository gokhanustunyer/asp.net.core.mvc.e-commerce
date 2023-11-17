using eticaret.business.Abstract.Storage.Azure;
using eticaret.business.Abstract.Storage.Local;
using eticaret.data.Abstract.Product;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.business.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandHandler
        : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        private readonly ILocalStorage _localStorage;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductRepository _productRepository;
        private readonly IConfiguration _configuration;

        public UploadProductImageCommandHandler(ILocalStorage localStorage,
                                                IProductImageRepository productImageRepository,
                                                IConfiguration configuration,
                                                IProductRepository productRepository)
        {
            _localStorage = localStorage;
            _productImageRepository = productImageRepository;
            _configuration = configuration;
            _productRepository = productRepository;
        }

        public async Task<UploadProductImageCommandResponse> Handle
                (UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            var datas = await _localStorage.UploadAsync(_configuration["Containers:Azure"], request.postedFiles);
            et.Product.Product product = await _productRepository.GetByIdAsync(request.id);
            await _productImageRepository.AddRangeAsync
                (
                    datas.Select(d => new et.Product.ProductImage()
                    {
                        FileName = d.fileName,
                        Path = d.path,
                        Storage = "Local",
                        Product = new List<et.Product.Product>() { product },
                        Index = request.index,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    }).ToList()
                );
            await _productImageRepository.SaveAsync();
            return new();
        }
    }
}
