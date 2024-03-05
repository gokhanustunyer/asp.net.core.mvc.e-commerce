using eticaret.business.Abstract.Storage.Local;
using eticaret.business.Concrete.Storage.Local;
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
        private readonly ILocalStorage _localStorage;

        public EditProductCommandHandler(IProductRepository productRepository,
                                         ICategoryRepository categoryRepository,
                                         IFilterRepository filterRepository,
                                         IConfiguration configuration,
                                         ILocalStorage localStorage)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _filterRepository = filterRepository;
            _configuration = configuration;
            _localStorage = localStorage;
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
            if (product == null) return new();
            
            List<et.Filter.Filter> filters = new();
            List<string> filterIds = new();
            if (request.FilterIds != null)
            {
                filterIds = request.FilterIds.Split(",").ToList();
                foreach (string filterId in filterIds)
                {
                    if (filterId != "")
                    {
                        filters.Add(await _filterRepository.GetByIdAsync(filterId));
                    }
                }
                product.Filters = filters;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(request.Description);
            if (request.DescFiles != null)
            {
                List<(string, string)> productDescImagePaths = await _localStorage.UploadAsync(_configuration["Containers:Azure"], request.DescFiles);
                List<ProductDescImage> productDescImages = new();
                for (int i = 0; i < productDescImagePaths.Count; i++)
                {
                    doc.DocumentNode.SelectNodes("//img[contains(@src, 'blob:')]")[i].Attributes[0].Value = _configuration["StoragePaths:Local"] + productDescImagePaths[i].Item2;
                    //productDescImages.Add(new()
                    //{
                    //    CreateDate = DateTime.Now,
                    //    UpdateDate = DateTime.Now,
                    //    Path = productDescImagePaths[i].Item2,
                    //    FileName = productDescImagePaths[i].Item1,
                    //    Storage = "Local",
                    //});
                }

                //if (product.ProductDescImages == null)
                //{
                //    product.ProductDescImages = new List<ProductDescImage>();
                //}

                //foreach (var img in productDescImages)
                //{
                //    product.ProductDescImages.Add(img);
                //}
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
            List<RelatedProduct> relatedIds = _productRepository.Context.RelatedProducts.Where(p => p.ProductId == product.Id.ToString() || p.RelatedProductId == product.Id.ToString()).ToList();
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

            product.Categories = categories;
            product.UpdateDate = DateTime.Now;
            product.Name = request.Name;
            product.Stock = request.Stock;
            product.Price = request.Price;
            product.Description = doc.DocumentNode.InnerHtml;
            product.ShortDescription = request.ShortDescription;
            product.RelatedProducts = relateds;

            _productRepository.Update(product);
            await _productRepository.SaveAsync();

            return new();
        }
    }
}
