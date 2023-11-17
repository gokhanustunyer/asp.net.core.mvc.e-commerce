using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.User.UpdateUserPassword
{
    public class SendUpdateUserPasswordMailCommandRequest: IRequest<SendUpdateUserPasswordMailCommandResponse>
    {
        public string UserName { get; set; }
    }
}
