using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Category.CreateCategory
{
    public class CreateCategoryCommandRequest: IRequest<CreateCategoryCommandResponse>
    {
        public string Name { get; set; }
        public string? TopCategoryId { get; set; }
        public IFormFile CategoryImage { get; set; }
    }
}
