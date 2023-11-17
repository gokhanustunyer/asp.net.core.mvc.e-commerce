using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.ProductImageFile.SaveAlignment
{
    public class SaveAlignmentCommandHandler : IRequestHandler<SaveAlignmentCommandRequest, SaveAlignmentCommandResponse>
    {
        public async Task<SaveAlignmentCommandResponse> Handle(SaveAlignmentCommandRequest request, CancellationToken cancellationToken)
        {
            return new();
        }
    }
}
