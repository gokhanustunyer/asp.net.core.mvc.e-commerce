using AutoMapper;
using eticaret.business.Abstract.Service;
using eticaret.business.Abstract.Storage.Azure;
using eticaret.business.Abstract.Storage.Local;
using eticaret.business.Operations;
using eticaret.business.ViewModels.Notice;
using eticaret.data.Abstract.Category;
using eticaret.data.Abstract.Filter;
using eticaret.data.Abstract.Product;
using eticaret.entity.Product;
using HtmlAgilityPack;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.business.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;
        private readonly ISizeService _sizeService;
        private readonly ILocalStorage _localStorage;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IFilterRepository _filterRepository;

        public CreateProductCommandHandler(IProductRepository productRepository,
                                           ICategoryRepository categoryRepository,
                                           ICategoryService categoryService,
                                           ISizeService sizeService,
                                           IMapper mapper,
                                           ILocalStorage localStorage,
                                           IConfiguration configuration,
                                           IFilterRepository filterRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _categoryService = categoryService;
            _sizeService = sizeService;
            _localStorage = localStorage;
            _configuration = configuration;
            _filterRepository = filterRepository;
        }

        public async Task<CreateProductCommandResponse> Handle
                (CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            if (request.filterIds != null && request.filterIds.Count > 0)
            {
                if (request.filterIds[0] != null)
                {
                    request.filterIds = request.filterIds[0].Split(",").ToList();
                    request.filterIds.RemoveAt(request.filterIds.Count() - 1);
                }
            }
            et.Product.Product product = _mapper.Map<et.Product.Product>(request);
            product.Id = Guid.NewGuid();

            product.Url = UrlNameOperation.CharacterRegulatory(request.Name,
                (string url) => _productRepository.Table.FirstOrDefault(p => p.Url == url) != null);

            List<et.Category.Category> categories = new();
            if (request.categoryIds != null)
            {
                for (var i = 0; i < request.categoryIds.Count(); i++)
                {   
                    et.Category.Category category 
                        = await _categoryRepository.GetByIdAsync
                          (request.categoryIds[i].ToString());
                    List<et.Category.Category> topCategories 
                        = await _categoryService.GetTopCategories(category);
                    categories.Add(category);
                    categories.AddRange(topCategories);
                }
            }
            List<(Size, int, float)> sizes = new();
            var sizesString = JsonConvert.DeserializeObject<List<OptionModel>>
                                                (request.OptionsAsJsonString);
            for (var i = 0; i < sizesString.Count; i++)
            {
                Size size = _sizeService.GetByName(sizesString[i].Name);
                if (size == null)
                    size = await _sizeService.GenerateSize(sizesString[i].Name);
                float price = (float)((sizesString[i].Price == null) ? request.Price : sizesString[i].Price);
                sizes.Add((size, sizesString[i].Count, price));
            }

            List<ProductSize> productSizes = new ();
            for (var i = 0; i < sizes.Count; i++)
            {
                productSizes.Add(new() {
                    Product = product, ProductId = product.Id, Stock = sizes[i].Item2,
                    SizeName = sizes[i].Item1.Name, SizeId = sizes[i].Item1.Id, Price = sizes[i].Item3
                } );
            }

            List<(string, string)> productImagePaths = await _localStorage.UploadAsync(_configuration["Containers:Azure"], request.postedFiles);            
            List<ProductImage> productImages = new();
            for (var i = 0; i < productImagePaths.Count; i++)
            {
                productImages.Add(new() 
                {
                    Id = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Path = productImagePaths[i].Item2,
                    FileName = productImagePaths[i].Item1,
                    Product = new() { product },
                    Storage = _configuration["ActiveStorage"],
                    Index = i
                });
            }

            List<(string, string)> productDescImagePaths = await _localStorage.UploadAsync(_configuration["Containers:Azure"], request.descFiles);
            List<ProductDescImage> productDescImages = new();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(request.Description);
            for (int i = 0; i < productDescImagePaths.Count; i++)
            {
                doc.DocumentNode.SelectNodes("//img[@src]")[i].Attributes[0].Value = _configuration["StoragePaths:Local"] + productDescImagePaths[i].Item2;
                productDescImages.Add(new()
                {
                    Id = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Path = productDescImagePaths[i].Item2,
                    FileName = productDescImagePaths[i].Item1,
                    Product = new() { product },
                    Storage = "Azure"
                });
            }
            List<RelatedProduct> relateds = new();
            List<string> relatedProductIds = new();
            if (request.RelatedProductIds != null)
            {
                relatedProductIds = request.RelatedProductIds.Split(",").ToList();
                for (int i = 0; i < relatedProductIds.Count-1; i++)
                {
                    relateds.Add(new() { 
                        ProductId = product.Id.ToString(),
                        RelatedProductId = relatedProductIds[i]
                    });
                }
            }
            List<et.Filter.Filter> filters = new();
            foreach(string filterId in request.filterIds)
            {
                filters.Add(await _filterRepository.GetByIdAsync(filterId));
            }

            product.Categories = categories;
            product.UpdateDate = DateTime.Now;
            product.CreateDate = DateTime.Now;
            product.ShortDescription = request.ShortDescription;
            product.Description = doc.DocumentNode.InnerHtml;
            product.ProductDescImages = productDescImages;
            product.MainImageId = productImages[0].Id.ToString();
            product.ProductImages = productImages;
            product.ProductSizes = productSizes;
            product.isActive = true;
            product.Filters = filters;
            product.RelatedProducts = relateds;
            var result = await _productRepository.AddAsync(product);
            await _productRepository.SaveAsync();


            NoticeViewModel noticeViewModel = null;
            if (result)
            {
                noticeViewModel = new NoticeViewModel()
                {
                    Title = "İşlem Başarılı",
                    Message = "Ürün Ekleme İşlemi Başarıyla Gerçekleştirildi",
                    MessageType = NoticeTypes.Success
                };
            }
            else
            {
                noticeViewModel = new NoticeViewModel()
                {
                    Title = "Beklenmedik Hata!",
                    Message = "İşlem Sırasında Beklenmedik Bir Sorun Oluştu",
                    MessageType = NoticeTypes.Error
                };
            }
            return new() { Notice = noticeViewModel };
        }


        public string CharacterRegulatory(string url)
        {
            url = FileNameOperation.CharacterRegulatory(url);
            url = url.ToLower();
            int counter = 0;
            while (_productRepository.Table
                .FirstOrDefault(p => p.Url == $"{url}-{counter}") == null)
                counter += 1;

            return (counter == 0) ? url : $"{url}-{counter}";
        }
    }
}
