using eticaret.entity.EntityRefrences.Discount;
using eticaret.entity.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Service
{
    public interface IDiscountService
    {
        List<DiscountCode> GetAll();
        Task<DiscountCode> GetByIdWithPropertiesAsync(string id);
        Task<EditDiscountCodeModel> GetForEditAsync(string id);
        Task<EditCampaignModel> GetCampaignForEditAsync(string id);
        Task<bool> DeleteProductFromCodeAsync(string productId, string codeId);
        List<Campaign> GetAllCapmaigns();
        Task<DiscountCode> CheckDiscountCodeAsync(string code, string UserName);
        Task<bool> RemoveDiscountFromBasket(string UserName);
        Task<bool> DeleteCampaignAsync(string id);
        Task<bool> RemoveProductFromCampaignAsync(string productId, string campaignId);
    }
}
