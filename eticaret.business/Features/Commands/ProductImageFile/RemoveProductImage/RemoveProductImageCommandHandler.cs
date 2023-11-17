using eticaret.data.Abstract.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.ProductImageFile.RemoveProductImage
{
    public class RemoveProductImageCommandHandler
            : IRequestHandler<RemoveProductImageCommandRequest, RemoveProductImageCommandResponse>
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductRepository _productRepository;

        public RemoveProductImageCommandHandler
            (IProductImageRepository productImageRepository,
             IProductRepository productRepository)
        {
            _productImageRepository = productImageRepository;
            _productRepository = productRepository;
        }

        public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product.MainImageId == request.ImageId)
                return new();
            await _productImageRepository.RemoveAsync(request.ImageId);
            await _productImageRepository.SaveAsync();

            return new();
        }
    }
}
