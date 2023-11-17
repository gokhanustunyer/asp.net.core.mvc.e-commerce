using AutoMapper;
using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.Cart;
using eticaret.data.Abstract.Discount;
using eticaret.data.Abstract.Product;
using eticaret.data.Contexts;
using eticaret.entity.Cart;
using eticaret.entity.EntityRefrences.Discount;
using eticaret.entity.Offer;
using eticaret.entity.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Concrete.Service
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        private readonly ETicaretDbContext _context;

        public DiscountService(IDiscountRepository discountRepository,
                               IMapper mapper,
                               ICategoryService categoryService,
                               IProductService productService,
                               IProductRepository productRepository,
                               ICartRepository cartRepository,
                               ETicaretDbContext context)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
            _categoryService = categoryService;
            _productService = productService;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _context = context;
        }
        public List<DiscountCode> GetAll()
            => _discountRepository.Table.OrderByDescending(dc => dc.CreateDate).ToList();
        public async Task<bool> DeleteProductFromCodeAsync(string productId, string codeId)
        {
            DiscountCode code = await _discountRepository.Table.Include(dc => dc.Products).FirstOrDefaultAsync(dc => dc.Id.ToString() == codeId);
            code.UpdateDate = DateTime.Now;
            Product product = await _productRepository.GetByIdAsync(productId);
            code.Products.Remove(product);
            await _discountRepository.SaveAsync();
            return true;
        }
        public async Task<DiscountCode> GetByIdWithPropertiesAsync(string id)
            => await _discountRepository.Table.Include(dc => dc.Products).Include(dc => dc.Categories).FirstOrDefaultAsync(dc => dc.Id.ToString() == id);
        public async Task<EditDiscountCodeModel> GetForEditAsync(string id)
        {
            DiscountCode code = await GetByIdWithPropertiesAsync(id);
            EditDiscountCodeModel model = _mapper.Map<EditDiscountCodeModel>(code);
            model.TopCategories = _categoryService.GetAllWithFullDeep();
            model.Products = await _productService.ProductToListModelRange(code.Products);
            return model;
        }
        public List<Campaign> GetAllCapmaigns()
            => _discountRepository.Context.Campaigns.ToList();
        public async Task<Campaign> GetCampaignByIdWithPropertiesAsync(string id)
            => await _discountRepository.Context.Campaigns.Include(dc => dc.Products).Include(dc => dc.Categories).FirstOrDefaultAsync(dc => dc.Id.ToString() == id);
        public async Task<EditCampaignModel> GetCampaignForEditAsync(string id)
        {
            Campaign code = await GetCampaignByIdWithPropertiesAsync(id);
            EditCampaignModel model = _mapper.Map<EditCampaignModel>(code);
            model.Code = code.Title;
            model.TopCategories = _categoryService.GetAllWithFullDeep();
            model.Products = await _productService.ProductToListModelRange(code.Products);
            return model;
        }
        public async Task<DiscountCode> CheckDiscountCodeAsync(string code, string UserName)
        {
            Cart cart = await _cartRepository.Table.Include(c => c.User).FirstOrDefaultAsync(c => c.User.UserName == UserName);
            DiscountCode discountCode = await _discountRepository.Table.FirstOrDefaultAsync(dc => dc.Code == code);
            if (discountCode == null)
            {
                throw new Exception();
            }
            if (discountCode.CodeEndDate.Year > 1)
            {
                if (DateTime.Now > discountCode.CodeEndDate)
                {
                    throw new Exception();
                }
                else
                {
                    discountCode.CodeLimitNumber = 0;
                }
            }
            else
            {
                if (discountCode.CodeLimitNumber == 0)
                {
                    throw new Exception();
                }
            }
            cart.DiscountCode = discountCode;
            _cartRepository.Update(cart);
            await _cartRepository.SaveAsync();
            return discountCode;
        }
        public async Task<bool> RemoveDiscountFromBasket(string UserName)
        {
            Cart cart = await _cartRepository.Table.Include(c => c.User).Include(c => c.DiscountCode).FirstOrDefaultAsync(c => c.User.UserName == UserName);
            DiscountCode code = await _discountRepository.Table.Include(dc => dc.Carts).FirstOrDefaultAsync(dc => dc.Id == cart.DiscountCode.Id);
            code.Carts.Remove(cart);
            _discountRepository.Update(code);
            await _discountRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteCampaignAsync(string id)
        {
            Campaign removeCampaign = await _context.Campaigns.Include(c => c.Categories).Include(c => c.Products).FirstOrDefaultAsync(c => c.Id.ToString() == id);
            _context.Campaigns.Remove(removeCampaign);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveProductFromCampaignAsync(string productId, string campaignId)
        {
            Campaign code = await _context.Campaigns.Include(dc => dc.Products).FirstOrDefaultAsync(dc => dc.Id.ToString() == campaignId);
            code.UpdateDate = DateTime.Now;
            Product product = await _productRepository.GetByIdAsync(productId);
            code.Products.Remove(product);
            await _discountRepository.SaveAsync();
            return true;
        }
    }
}
