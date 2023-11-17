using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.ProductImageFile.RemoveProductImage
{
    public class RemoveProductImageCommandRequest: IRequest<RemoveProductImageCommandResponse>
    {
        public string ImageId { get; set; }
        public string ProductId { get; set; }
    }
}
