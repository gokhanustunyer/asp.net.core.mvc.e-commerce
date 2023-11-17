using eticaret.business.Abstract.Service;
using eticaret.business.ViewModels.Notice;
using eticaret.entity.EntityRefrences.UserReference;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {
        private readonly IUserService _userService;

        public LoginCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            NoticeViewModel noticeViewModel = null;
            if (request.UserName == null || request.Password == null)
            {
                return new()
                {
                    Notice = new NoticeViewModel()
                    {
                        Title = "Hatalı Giriş!",
                        Message = "Kullanıcı adı veya parola alanı boş geçilemez",
                        MessageType = NoticeTypes.Error
                    }
                };
            }
            var model = new UserLoginModel() 
            { 
                Password = request.Password,
                UserName = request.UserName
            };
            bool result = await _userService.LoginAsync(model);
            if (!result)
            {
                noticeViewModel = new NoticeViewModel()
                {
                    Title = "Hatalı Giriş!",
                    Message = "Kullanıcı adı veya parola hatalı",
                    MessageType = NoticeTypes.Error
                };
            }
            else
            {
                noticeViewModel = new NoticeViewModel()
                {
                    Title = "Giriş Başarılı!",
                    Message = "Gezinmenin tadını çıkartın",
                    MessageType = NoticeTypes.Success
                };
            }
            return new()
            {
                Notice = noticeViewModel
            };
        }
    }
}
