using eticaret.business.Features.Commands.User.UpdateUserEmail;
using eticaret.entity.EntityRefrences.OrderReference;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.EntityRefrences.UserReference;
using eticaret.entity.Identity;
using eticaret.entity.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Service
{
    public interface IUserService
    {
        Task<bool> CreateAsync(CreateUserModel model);
        Task<bool> LoginAsync(UserLoginModel model);
        Task<(List<UserListModel>, List<UserListModel>, AppRole)> GetRoleMembersWithNonMembers(string roleId);
        Task<bool> LogoutAsync();
        Task<bool> AddToRoleAsync(string userId, string roleId);
        Task<AppUser> GetFullUserByName(string username);
        AppUser GetCommentsWithRateAsync(string username);
        Task<List<ProductCommentCartModel>> GetProductsByUserComments(string username);
        Task<bool> ConfirmEmail(string userId, string token);
        Task<bool> ConfirmUpdateEmail(string userId, string token, string newEmail);
        Task<UserCheckOutModel> GetCheckOut(string username);
        Task<UserOrderModel> GetOrdersAsync(string username);
        Task<string> ExternalLoginAsync();
        Task<bool> SendResetPasswordRequest(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordModel model);
    }
}
