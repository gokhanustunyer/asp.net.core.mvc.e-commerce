using eticaret.entity.EntityRefrences.FileReference;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Service
{
    public interface IProductImageService
    {
        Task<List<ProductImageShowModel>> GetByProductIdAsync(string productId);
        string GetMainImagePathByProductId(string productId);
        Task<bool> ReIndex(List<ImageAndIndexsModel> alignment, string productId);
    }
}
