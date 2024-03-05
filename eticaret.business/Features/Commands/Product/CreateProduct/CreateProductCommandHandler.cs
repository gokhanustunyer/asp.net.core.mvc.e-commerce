using AutoMapper;
using eticaret.business.Abstract.Service;
using eticaret.business.Abstract.Storage.Azure;
using eticaret.business.Abstract.Storage.Local;
using eticaret.business.Operations;
using eticaret.business.ViewModels.Notice;
using eticaret.data.Abstract.Category;
using eticaret.data.Abstract.Filter;
using eticaret.data.Abstract.Product;
using eticaret.entity.EntityRefrences.ProductCategoryReference;
using eticaret.entity.Identity;
using eticaret.entity.Product;
using HtmlAgilityPack;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        public CreateProductCommandHandler(IProductRepository productRepository,
                                           ICategoryRepository categoryRepository,
                                           ICategoryService categoryService,
                                           ISizeService sizeService,
                                           IMapper mapper,
                                           ILocalStorage localStorage,
                                           IConfiguration configuration,
                                           IFilterRepository filterRepository,
                                           UserManager<AppUser> userManager)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _categoryService = categoryService;
            _sizeService = sizeService;
            _localStorage = localStorage;
            _configuration = configuration;
            _filterRepository = filterRepository;
            _userManager = userManager;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            AppUser publisher = await _userManager.FindByNameAsync(request.PublisherAsString);
            et.Product.Product product = _mapper.Map<et.Product.Product>(request);
            product.Publisher = publisher;
            product.Id = Guid.NewGuid();

            product.Url = UrlNameOperation.CharacterRegulatory(request.Name, (string url) => _productRepository.Table.FirstOrDefault(p => p.Url == url) != null);

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
            var sizesString = JsonConvert.DeserializeObject<List<OptionModel>>(request.OptionsAsJsonString);
            if (sizesString != null)
            {
                for (var i = 0; i < sizesString.Count; i++)
                {
                    float price = (float)((sizesString[i].Price == null) ? request.Price : sizesString[i].Price);
                    
                    Size size = _sizeService.GetByName(sizesString[i].Name);
                    if (size == null)
                    {
                        size = await _sizeService.GenerateSize(sizesString[i].Name);
                    }

                    sizes.Add((size, sizesString[i].Count, price));
                }
            }

            List<ProductSize> productSizes = new ();
            for (var i = 0; i < sizes.Count; i++)
            {
                productSizes.Add(new() {
                    Product = product, ProductId = product.Id, Stock = sizes[i].Item2,
                    SizeName = sizes[i].Item1.Name, SizeId = sizes[i].Item1.Id, Price = sizes[i].Item3
                } );
            }

            List<ProductImage> productImages = new();
            if (request.postedFiles != null)
            {
                List<(string, string)> productImagePaths = await _localStorage.UploadAsync(_configuration["Containers:Azure"], request.postedFiles);            
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
            }

            List<(string, string)> productDescImagePaths = new();
            List<ProductDescImage> productDescImages = new();
            HtmlDocument descriptionDocument = new HtmlDocument();
            descriptionDocument.LoadHtml(request.Description);
            if (request.descFiles != null)
            {
                productDescImagePaths = await _localStorage.UploadAsync(_configuration["Containers:Azure"], request.descFiles);
                for (int i = 0; i < productDescImagePaths.Count; i++)
                {
                    descriptionDocument.DocumentNode.SelectNodes("//img[@src]")[i].Attributes[0].Value = _configuration["StoragePaths:Local"] + productDescImagePaths[i].Item2;
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
            List<string> filterIds = new();
            if (request.filterIds != null)
            {
                filterIds = request.filterIds.Split(",").ToList();
                foreach (string filterId in filterIds)
                {
                    if (filterId != "")
                    {
                        filters.Add(await _filterRepository.GetByIdAsync(filterId));
                    }
                }
            }

            product.Categories = categories;
            product.UpdateDate = DateTime.Now;
            product.CreateDate = DateTime.Now;
            product.ShortDescription = request.ShortDescription;
            product.Description = descriptionDocument?.DocumentNode.InnerHtml;
            product.ProductDescImages = productDescImages;
            product.MainImageId = productImages?[0].Id.ToString();
            product.ProductImages = productImages;
            product.ProductSizes = productSizes;
            product.isActive = true;
            product.Filters = filters;
            product.RelatedProducts = relateds;
            product.Publisher = publisher;
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
