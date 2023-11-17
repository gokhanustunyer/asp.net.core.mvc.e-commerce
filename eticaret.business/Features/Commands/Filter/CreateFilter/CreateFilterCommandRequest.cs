using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Filter.CreateFilter
{
    public class CreateFilterCommandRequest: IRequest<CreateFilterCommandResponse>
    {
        public string[] CategoryIds { get; set; }
        public string[] FilterNames { get; set; }
        public string FilterBoxTitle { get; set; }
    }
}
