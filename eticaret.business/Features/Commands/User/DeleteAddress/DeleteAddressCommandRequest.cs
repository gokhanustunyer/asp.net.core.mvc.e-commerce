using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.User.DeleteAddress
{
    public class DeleteAddressCommandRequest: IRequest<DeleteAddressCommandResponse>
    {
        public string AddressId { get; set; }
    }
}
