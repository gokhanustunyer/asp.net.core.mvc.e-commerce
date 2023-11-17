using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.User.UpdateUser
{
    public class UpdateUserCommandRequest: IRequest<UpdateUserCommandResponse>
    {
        public string userName { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string neighborhood { get; set; }
        public string detailedAddress { get; set; }
    }
}
