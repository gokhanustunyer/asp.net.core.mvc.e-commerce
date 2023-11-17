using eticaret.data.Abstract.Product;
using eticaret.entity.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;
using et = eticaret.entity;

namespace eticaret.business.Features.Commands.Product.UpdateSize
{
    public class UpdateSizeCommandHandler : IRequestHandler<UpdateSizeCommandRequest, UpdateSizeCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        public UpdateSizeCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<UpdateSizeCommandResponse> Handle(UpdateSizeCommandRequest request, CancellationToken cancellationToken)
        {
            et.Product.Product? product = await _productRepository.Table.Include(p => p.ProductSizes).FirstOrDefaultAsync(p => p.Id.ToString() == request.ProductId);
            product.ProductSizes.FirstOrDefault(ps => ps.SizeId.ToString() == request.SizeId).Stock = request.Stock;
            product.ProductSizes.FirstOrDefault(ps => ps.SizeId.ToString() == request.SizeId).Price = request.Price;
            _productRepository.Update(product);
            await _productRepository.SaveAsync();
            return new();
        }
    }
}
