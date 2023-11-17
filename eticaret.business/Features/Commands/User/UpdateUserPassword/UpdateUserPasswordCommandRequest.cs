using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.User.UpdateUserPassword
{
    public class UpdateUserPasswordCommandRequest: IRequest<UpdateUserPasswordCommandResponse>
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordAgain { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
