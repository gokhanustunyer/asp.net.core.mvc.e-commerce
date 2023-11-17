﻿using eticaret.data.Abstract.Category;
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

namespace eticaret.business.Features.Commands.Discount.CreateDiscountCode
{
    public class CreateDiscountCodeCommandHandler : IRequestHandler<CreateDiscountCodeCommandRequest, CreateDiscountCodeCommandResponse>
    {
        private readonly ETicaretDbContext _context;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CreateDiscountCodeCommandHandler(ETicaretDbContext context,
                                                ICategoryRepository categoryRepository,
                                                IProductRepository productRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<CreateDiscountCodeCommandResponse> Handle(CreateDiscountCodeCommandRequest request, CancellationToken cancellationToken)
        {
            if (_context.DiscountCodes.FirstOrDefault(c => c.Code == request.Code) != null)
            {
                return new();
            }
            DiscountCode discountCode = new()
            {
                Code = request.Code,
                CodeEndDate = request.CodeEndDate,
                CodeStartDate = request.CodeStartDate,
                CodeLimitNumber = request.CodeLimitNumber,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Id = Guid.NewGuid(),
                DiscountNumber = request.DiscountNumber,
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
                foreach(string categoryId in request.CategoryIds) 
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

            await _context.DiscountCodes.AddAsync(discountCode);
            await _context.SaveChangesAsync();
            return new();
        }
    }
}
