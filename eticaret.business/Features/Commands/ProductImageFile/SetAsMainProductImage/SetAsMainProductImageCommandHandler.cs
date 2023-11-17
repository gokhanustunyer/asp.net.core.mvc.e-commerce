using eticaret.data.Abstract.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.business.Features.Commands.ProductImageFile.SetAsMainProductImage
{
    public class SetAsMainProductImageCommandHandler
        : IRequestHandler<SetAsMainProductImageCommandRequest, SetAsMainProductImageCommandResponse>
    {
        private readonly IProductRepository _productRepository;

        public SetAsMainProductImageCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<SetAsMainProductImageCommandResponse> Handle(SetAsMainProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            et.Product.Product product =
                await _productRepository.GetByIdAsync(request.ProductId);
            product.MainImageId = request.ImageId;
            _productRepository.Update(product);
            await _productRepository.SaveAsync();

            return new();
        }
    }
}
