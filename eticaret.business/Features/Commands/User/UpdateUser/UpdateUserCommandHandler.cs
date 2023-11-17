using eticaret.business.Abstract.Service;
using eticaret.data.Services.Abstract;
using eticaret.entity.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.User.UpdateUser
{
    public class UpdateUserCommandHandler
        : IRequestHandler<UpdateUserCommandRequest, UpdateUserCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICitiesDbService _citiesDbService;

        public UpdateUserCommandHandler(UserManager<AppUser> userManager,
                                        ICitiesDbService cityDbService)
        {
            _userManager = userManager;
            _citiesDbService = cityDbService;
        }


        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.userName);
            user.FirstName = request.firstName;
            user.LastName = request.lastName;
            await _userManager.UpdateAsync(user);
            return new();
        }
    }
}
