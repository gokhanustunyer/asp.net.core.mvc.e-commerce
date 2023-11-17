using eticaret.business.Abstract.Service;
using eticaret.data.Contexts;
using eticaret.entity.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Queries.User.GetUserAddressQuery
{
    public class GetUserAddressQueryHandler
        : IRequestHandler<GetUserAddressQueryRequest, GetUserAddressQueryResponse>
    {
        private readonly IUserService _userService;

        public GetUserAddressQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetUserAddressQueryResponse> Handle
            (GetUserAddressQueryRequest request, CancellationToken cancellationToken)
        {
            AppUser user = await _userService.GetFullUserByName(request.UserName);
            return new() { Addresses = user.Addresses.ToList() };
        }
    }
}
