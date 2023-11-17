using eticaret.business.Abstract.Storage.Azure;
using eticaret.business.Operations;
using eticaret.data.Abstract.Category;
using eticaret.data.Abstract.Filter;
using eticaret.data.Abstract.Product;
using eticaret.entity.Product;
using HtmlAgilityPack;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.business.Features.Commands.Product.EditProduct
{
    public class EditProductCommandHandler
            : IRequestHandler<EditProductCommandRequest, EditProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFilterRepository _filterRepository;
        private readonly IConfiguration _configuration;
        private readonly IAzureStorage _azureStorage;

        public EditProductCommandHandler(IProductRepository productRepository,
                                         ICategoryRepository categoryRepository,
                                         IFilterRepository filterRepository,
                                         IConfiguration configuration,
                                         IAzureStorage azureStorage)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _filterRepository = filterRepository;
            _configuration = configuration;
            _azureStorage = azureStorage;
        }

        public async Task<EditProductCommandResponse> Handle
                (EditProductCommandRequest request, CancellationToken cancellationToken)
        {
            et.Product.Product? product = await _productRepository.Table
                                                                  .Include(p => p.ProductDescImages)
                                                                  .Include(p => p.Filters)
                                                                  .Include(p => p.RelatedProducts)
                                                                  .Include(d => d.Categories)
                                                                  .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.id));

            if (request.FilterIds != null && request.FilterIds.Count > 0)
            {
                if (request.FilterIds[0] != null)
                {
                    request.FilterIds = request.FilterIds[0].Split(",").ToList();
                    request.FilterIds.RemoveAt(request.FilterIds.Count() - 1);
                }
            }
            List<et.Filter.Filter> filters = new();
            foreach (string filterId in request.FilterIds)
            {
                filters.Add(await _filterRepository.GetByIdAsync(filterId));
            }

            List<(string, string)> productDescImagePaths
                = await _azureStorage.UploadAsync(_configuration
                       ["Containers:Azure"], request.DescFiles);
            List<ProductDescImage> productDescImages = new();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(request.Description);
            for (int i = 0; i < productDescImagePaths.Count; i++)
            {
                doc.DocumentNode.SelectNodes("//img[contains(@src, 'blob:')]")[i].Attributes[0].Value
                    = _configuration["StoragePaths:Azure"] + productDescImagePaths[i].Item2;
                productDescImages.Add(new()
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Path = productDescImagePaths[i].Item2,
                    FileName = productDescImagePaths[i].Item1,
                    Product = new() { product },
                    Storage = "Azure",
                });
            }

            List<et.Category.Category> categories = new();
            for (var i = 0; i < request.categoryIds.Count(); i++)
            {
                et.Category.Category category
                    = await _categoryRepository.GetByIdAsync
                      (request.categoryIds[i].ToString());
                categories.Add(category);
            }


            List<RelatedProduct> relateds = new();
            List<string> relatedProductIds = new();
            if (request.RelatedProductIds != null)
            {
                relatedProductIds = request.RelatedProductIds.Split(",").ToList();
                for (int i = 0; i < relatedProductIds.Count - 1; i++)
                {
                    relateds.Add(new()
                    {
                        ProductId = request.id.ToString(),
                        RelatedProductId = relatedProductIds[i]
                    });
                }
            }
            List<RelatedProduct> relatedIds = _productRepository.Context
                                               .RelatedProducts.Where
                                               (p => p.ProductId == product.Id.ToString() ||
                                         p.RelatedProductId == product.Id.ToString()).ToList();
            foreach (RelatedProduct item in relatedIds)
            {
                var relatedModel = await _productRepository.Context.RelatedProducts.FirstOrDefaultAsync(r => r.RelatedProductId == item.RelatedProductId && r.ProductId == item.ProductId);
                _productRepository.Context.RelatedProducts.Remove(relatedModel);
            }
            await _productRepository.Context.SaveChangesAsync();

            if (request.Url != product.Url)
            {
                product.Url = UrlNameOperation.CharacterRegulatory(request.Url,
                    (string url) => _productRepository.Table.FirstOrDefault(p => p.Url == url) != null);
            }

            if (product != null)
            {
                foreach(var img in productDescImages) 
                { 
                    product.ProductDescImages.Add(img);
                }
                product.Categories = categories;
                product.UpdateDate = DateTime.Now;
                product.Name = request.Name;
                product.Stock = request.Stock;
                product.Price = request.Price;
                product.Description = doc.DocumentNode.InnerHtml;
                product.ShortDescription = request.ShortDescription;
                product.RelatedProducts = relateds;
                product.Filters = filters;
                _productRepository.Update(product);
                await _productRepository.SaveAsync();
            }
            return new();
        }
    }
}
