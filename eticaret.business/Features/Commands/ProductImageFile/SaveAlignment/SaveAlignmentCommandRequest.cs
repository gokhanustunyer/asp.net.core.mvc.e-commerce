using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.ProductImageFile.SaveAlignment
{
    public class SaveAlignmentCommandRequest: IRequest<SaveAlignmentCommandResponse>
    {
        public string imageId { get; set; }
        public int index { get; set; }
    }
}
