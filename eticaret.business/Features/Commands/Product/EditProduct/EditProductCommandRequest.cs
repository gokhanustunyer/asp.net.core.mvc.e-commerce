using eticaret.entity.Product;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.business.Features.Commands.Product.EditProduct
{
    public class EditProductCommandRequest: IRequest<EditProductCommandResponse>
    {
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public string? Url { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<et.Category.Category> ActiveCategories { get; set; }
        public List<et.Category.Category> AllCategories { get; set; }
        public IFormFileCollection DescFiles { get; set; }
        public string RelatedProductIds { get; set; }
        public List<string> FilterIds { get; set; }
        public string Path { get; set; }
        public Guid[] categoryIds { get; set; }
        public string id { get; set; }
    }
}
