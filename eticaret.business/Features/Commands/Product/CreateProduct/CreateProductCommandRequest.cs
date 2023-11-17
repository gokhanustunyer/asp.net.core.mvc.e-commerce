using eticaret.entity.Category;
using eticaret.entity.EntityRefrences.CategoryReference;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.business.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandRequest: IRequest<CreateProductCommandResponse>
    {
        [Required(ErrorMessage = "İsim Alanı Zorunludur")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }

        [Required(ErrorMessage = "Fiyat Alanı Zorunludur")]
        public double Price { get; set; }
        public string? RelatedProductIds { get; set; }
        public int? Stock { get; set; }
        public string OptionsAsJsonString { get; set; }
        public List<string> filterIds { get; set; }
        public Guid[]? categoryIds { get; set; }
        public ICollection<et.Category.Category>? Categories { get; set; }
        public IFormFileCollection? postedFiles { get; set; }
        public IFormFileCollection? descFiles { get; set; }

    }
}
