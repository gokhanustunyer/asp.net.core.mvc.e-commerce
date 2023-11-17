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

namespace eticaret.business.Features.Commands.User.UpdateUserEmail
{
    public class UpdateUserEmailCommandHandler
        : IRequestHandler<UpdateUserEmailCommandRequest, UpdateUserEmailCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public UpdateUserEmailCommandHandler(UserManager<AppUser> userManager,
                                             IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<UpdateUserEmailCommandResponse> Handle
            (UpdateUserEmailCommandRequest request, CancellationToken cancellationToken)
        {
            if (request.NewEmail != request.NewEmailAgain) { return new(); }
            AppUser user1 = await _userManager.FindByNameAsync(request.UserName);
            AppUser user2 = await _userManager.FindByEmailAsync(request.Email);
            if (user1 != user2) { return new(); }
            bool pswCheck = await _userManager.CheckPasswordAsync(user1, request.Password);
            if (!pswCheck) { return new(); }
            string token = await _userManager.GenerateChangeEmailTokenAsync(user1, request.NewEmail);
            string tokenUrl = $"/User/ConfirmUpdateEmail?userId={user1.Id}&token={HttpUtility.UrlEncode(token)}&newEmail={request.NewEmail}";
            await _emailService.SendEmailAsync(request.NewEmail, "E-Posta Adresinizi Değiştirin", tokenUrl);

            return new();
        }
    }
}
