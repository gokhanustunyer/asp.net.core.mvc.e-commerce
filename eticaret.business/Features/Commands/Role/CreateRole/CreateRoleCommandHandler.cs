using eticaret.entity.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Role.CreateRole
{
    public class CreateRoleCommandHandler
            : IRequestHandler<CreateRoleCommandRequest, CreateRoleCommandReponse>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public CreateRoleCommandHandler(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<CreateRoleCommandReponse> Handle(CreateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            await _roleManager.CreateAsync(new AppRole() { Name = request.Name });
            return new();
        }
    }
}
