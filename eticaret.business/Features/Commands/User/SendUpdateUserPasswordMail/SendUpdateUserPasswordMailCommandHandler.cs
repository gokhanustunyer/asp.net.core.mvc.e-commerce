using eticaret.business.Abstract.Service;
using eticaret.entity.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eticaret.business.Features.Commands.User.UpdateUserPassword
{
    public class SendUpdateUserPasswordMailCommandHandler
        : IRequestHandler<SendUpdateUserPasswordMailCommandRequest, SendUpdateUserPasswordMailCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IEmailService _emailService;

        public SendUpdateUserPasswordMailCommandHandler(UserManager<AppUser> userManager,
                                                SignInManager<AppUser> signInManager,
                                                IEmailService emailService)
        {
            _userManager = userManager;
            this.signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<SendUpdateUserPasswordMailCommandResponse> Handle
            (SendUpdateUserPasswordMailCommandRequest request, CancellationToken cancellationToken)
        {
            AppUser user = await _userManager.FindByNameAsync(request.UserName);
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string url = $"/user/updatePassword/?token={HttpUtility.UrlEncode(token)}";
            await _emailService.SendEmailAsync(user.Email, "Şifre Yenileme", url);


            return new();
        }
    }
}
