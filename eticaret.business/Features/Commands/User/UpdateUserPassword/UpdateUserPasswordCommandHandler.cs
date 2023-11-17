using eticaret.entity.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.User.UpdateUserPassword
{
    public class UpdateUserPasswordCommandHandler
        : IRequestHandler<UpdateUserPasswordCommandRequest, UpdateUserPasswordCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UpdateUserPasswordCommandHandler(UserManager<AppUser> userManager, 
                                                SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<UpdateUserPasswordCommandResponse> Handle(UpdateUserPasswordCommandRequest request, CancellationToken cancellationToken)
        {
            if (request.NewPassword != request.NewPasswordAgain) { return new(); }
            AppUser user = await _userManager.FindByNameAsync(request.UserName);
            bool pswCheck = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);
            if (!pswCheck) { return new(); }
            IdentityResult result = await _userManager.ResetPasswordAsync
                                        (user, request.Token, request.NewPassword);

            if (!result.Succeeded) { return new(); }
            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, true);
            return new();
        }
    }
}
