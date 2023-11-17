using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Product.UpdateSize
{
    public class UpdateSizeCommandRequest: IRequest<UpdateSizeCommandResponse>
    {
        public string SizeId { get; set; }
        public string ProductId { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
    }
}
