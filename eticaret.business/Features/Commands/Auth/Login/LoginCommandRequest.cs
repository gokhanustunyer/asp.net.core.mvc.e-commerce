using MediatR;

namespace eticaret.business.Features.Commands.Auth.Login
{
    public class LoginCommandRequest: IRequest<LoginCommandResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}