using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.Discount;
using eticaret.data.Abstract.Product;
using eticaret.entity.Offer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.business.Features.Commands.Discount.EditDiscountCode
{
    public class EditDiscountCodeCommandHandler : IRequestHandler<EditDiscountCodeCommandRequest, EditDiscountCodeCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;

        public EditDiscountCodeCommandHandler(IProductRepository productRepository, 
                                              IDiscountRepository discountRepository)
        {
            _productRepository = productRepository;
            _discountRepository = discountRepository;
        }

        public async Task<EditDiscountCodeCommandResponse> Handle(EditDiscountCodeCommandRequest request, CancellationToken cancellationToken)
        {
            DiscountCode code = await _discountRepository.Table.Include(dc => dc.Products).FirstOrDefaultAsync(dc => dc.Id.ToString() == request.Id);
            
            if (request.SelectedProductIds != null)
            {
                foreach(string productId in request.SelectedProductIds.Split(","))
                {
                    if(productId != "" && productId != null)
                    {
                        et.Product.Product product = await _productRepository.GetByIdAsync(productId);
                        code.Products.Add(product);
                    }
                }
            }
            code.UpdateDate = DateTime.Now;
            code.CodeEndDate = request.CodeEndDate;
            code.CodeStartDate = request.CodeStartDate;
            code.Code = request.Code;
            code.CodeLimitNumber = request.CodeLimitNumber;
            code.DiscountNumber = request.DiscountNumber;
            code.DiscountRate = request.DiscountRate;
            _discountRepository.Update(code);
            await _discountRepository.SaveAsync();
            return new();
        }
    }
}
