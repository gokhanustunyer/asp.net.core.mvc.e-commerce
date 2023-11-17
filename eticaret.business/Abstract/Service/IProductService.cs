using eticaret.entity.EntityRefrences.ProductCategoryReference;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Identity;
using eticaret.entity.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Service
{
    public interface IProductService
    {
        Task<List<ProductListModel>> GetAll();
        Task<bool> CreateAsync(ProductCreateModel model, Guid[] categoryIds);
        Task<bool> EditAsync(string id, ProductEditModel p, Guid[] categoryIds);
        Task<bool> RemoveAsync(string id);
        Task<ProductEditModel> GetWithAllProperties(string id);
        Task<bool> AddImage(IFormFileCollection postedFiles, string id);
        Task<bool> DeleteImage(string id);
        Task<ProductCategoryPageModel> GetLastProducts(string category, string order);
        Task<ProductCategoryPageModel> GetProductsByFilters(string category, string order, string filterQuery);
        Task<ProductDetailsModel> GetProductDetails(string id);
        Task<ProductListModel> GetByIdWithImage(string id);
        Task<List<ProductListModel>> GetAllWithImages();
        Task<ProductListModel> ProductToListModel(Product product);
        Task<List<ProductCommentModel>> GetCommentsAsync(string productId, bool haveImage);
        Task<List<ProductListModel>> Search(string searchWords);
        Task<bool> DeleteSizeOnProduct(string sizeId, string productId);
        Task<List<ProductSize>> GetSizesById(string productId);
        Task<bool> AddNewOption(string ProductId, string SizeName, double Price, int Stock);
        Task<List<ProductListModel>> ProductToListModelRange(List<Product> products);
        Task<List<ProductListModel>> GetMostSellings(int productCount);
        Task<List<ProductListModel>> GetProductsByCategoryIds(string CategoryIds);
        Task<List<ProductListModel>> GetProductsByCategoryIds(string CategoryIds, int count);
        Task<List<ProductQAListModel>> GetQAs(string id);
        Task<Product> CreateQa(ProductQaCreateModel model);
        Task<AppUser> GetProductPublisherById(string productId);
        Task<Product> SendQaResponse(string message, string productId, string qaId, string username);
        Task<List<ProductListModel>> GetProductsByIds(IEnumerable<string> productIds);
        Task<List<ProductListModel>> GetProductsBasedOnSimilarFiltersByCategory(string categoryId, string productId);
    }
}
