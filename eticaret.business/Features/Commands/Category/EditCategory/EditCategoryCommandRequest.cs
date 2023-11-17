using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Category.EditCategory
{
    public class EditCategoryCommandRequest: IRequest<EditCategoryCommandResponse>
    {
        public string Id { get; set; }
        public Guid[] SubCategoryIds { get; set; }
        public Guid TopCategoryIds { get; set; }
        public IFormFile? CategoryImage { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
