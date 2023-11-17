using eticaret.data.Abstract.Category;
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

namespace eticaret.business.Features.Commands.Discount.CreateCampaign
{
    public class CreateCampaignCommandHandler : IRequestHandler<CreateCampaignCommandRequest, CreateCampaignCommandResponse>
    {
        private readonly ETicaretDbContext _context;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CreateCampaignCommandHandler(ETicaretDbContext context,
                                                ICategoryRepository categoryRepository,
                                                IProductRepository productRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<CreateCampaignCommandResponse> Handle(CreateCampaignCommandRequest request, CancellationToken cancellationToken)
        {
            if (_context.DiscountCodes.FirstOrDefault(c => c.Code == request.Code) != null)
            {
                return new();
            }
            Campaign discountCode = new()
            {
                Title = request.Code,
                CodeEndDate = request.CodeEndDate,
                CodeStartDate = request.CodeStartDate,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Id = Guid.NewGuid(),
                DiscountRate = request.DiscountRate,
            };

            if (request.CategoryIds == null)
            {
                discountCode.Categories = _context.Categories.ToList();
                discountCode.Products = _context.Products.ToList();
            }
            else
            {
                List<et.Category.Category> categories = new();
                foreach (string categoryId in request.CategoryIds)
                {
                    categories.Add(await _categoryRepository.Table.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id.ToString() == categoryId));
                }

                List<et.Product.Product> products = new();
                if (request.SelectedProductIds == null)
                {
                    foreach (et.Category.Category category in categories)
                    {
                        products.AddRange(category.Products.ToList());
                    }
                }
                else
                {
                    foreach (string productId in request.SelectedProductIds.Split(","))
                    {
                        if (productId != "" && productId != null)
                        {
                            products.Add(await _productRepository.GetByIdAsync(productId));
                        }
                    }
                }
                discountCode.Products = products;
            }

            await _context.Campaigns.AddAsync(discountCode);
            await _context.SaveChangesAsync();
            return new();
        }
    }
}
