using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.User.UpdateUserEmail
{
    public class UpdateUserEmailCommandRequest: IRequest<UpdateUserEmailCommandResponse>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string NewEmail { get; set; }
        public string NewEmailAgain { get; set; }
        public string Password { get; set; }
    }
}
