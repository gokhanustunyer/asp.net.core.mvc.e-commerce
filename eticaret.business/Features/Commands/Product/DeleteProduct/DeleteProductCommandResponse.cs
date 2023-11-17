using eticaret.business.ViewModels.Notice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Product.DeleteProduct
{
    public class DeleteProductCommandResponse
    {
        public NoticeViewModel? Notice { get; set; }
    }
}
