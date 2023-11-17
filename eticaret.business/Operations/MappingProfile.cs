using AutoMapper;
using eticaret.business.Features.Commands.Product.CreateProduct;
using eticaret.entity.Category;
using eticaret.entity.EntityRefrences.CategoryReference;
using eticaret.entity.EntityRefrences.Discount;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Offer;
using eticaret.entity.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Operations
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductListModel, Product>();
            CreateMap<Product, ProductListModel>();
            CreateMap<Product, ProductEditModel>();
            CreateMap<CreateProductCommandRequest, Product>();
            CreateMap<ProductCreateModel, Product>();
            CreateMap<Product, ProductCreateModel>();


            CreateMap<CategoryListModel, Category>();
            CreateMap<Category, CategoryListModel>();
            CreateMap<Category, CategoryEditModel>();
            CreateMap<DiscountCode, EditDiscountCodeModel>();
            CreateMap<Campaign, EditCampaignModel>();


        }
    }
}
