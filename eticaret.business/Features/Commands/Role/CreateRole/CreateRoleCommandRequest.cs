using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Role.CreateRole
{
    public class CreateRoleCommandRequest: IRequest<CreateRoleCommandReponse>
    {
        public string Name { get; set; }
    }
}
