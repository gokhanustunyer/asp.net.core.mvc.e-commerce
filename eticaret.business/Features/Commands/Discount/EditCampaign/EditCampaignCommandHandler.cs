using eticaret.data.Abstract.Discount;
using eticaret.data.Abstract.Product;
using eticaret.data.Contexts;
using eticaret.entity.Offer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.business.Features.Commands.Discount.EditCampaign
{
    public class EditCampaignCommandHandler : IRequestHandler<EditCampaignCommandRequest, EditCampaignCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly ETicaretDbContext _context;
        public EditCampaignCommandHandler(IProductRepository productRepository,
                                          ETicaretDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<EditCampaignCommandResponse> Handle(EditCampaignCommandRequest request, CancellationToken cancellationToken)
        {
            Campaign code = await _context.Campaigns.Include(dc => dc.Products).FirstOrDefaultAsync(dc => dc.Id.ToString() == request.Id);

            if (request.SelectedProductIds != null)
            {
                foreach (string productId in request.SelectedProductIds.Split(","))
                {
                    if (productId != "" && productId != null)
                    {
                        et.Product.Product product = await _productRepository.GetByIdAsync(productId);
                        code.Products.Add(product);
                    }
                }
            }
            code.UpdateDate = DateTime.Now;
            code.CodeEndDate = request.CodeEndDate;
            code.CodeStartDate = request.CodeStartDate;
            code.Title = request.Code;
            code.DiscountRate = request.DiscountRate;
            _context.Campaigns.Update(code);
            await _context.SaveChangesAsync();
            return new();
        }
    }
}
