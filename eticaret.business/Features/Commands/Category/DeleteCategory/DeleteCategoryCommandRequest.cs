using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Category.DeleteCategory
{
    public class DeleteCategoryCommandRequest: IRequest<DeleteCategoryCommandResponse>
    {
        public string Id { get; set; }
    }
}
