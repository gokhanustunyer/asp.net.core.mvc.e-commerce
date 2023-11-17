using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandRequest: IRequest<UploadProductImageCommandResponse>
    {
        public string id { get; set; }
        public int index { get; set; }
        public IFormFileCollection postedFiles { get; set; }
    }
}
