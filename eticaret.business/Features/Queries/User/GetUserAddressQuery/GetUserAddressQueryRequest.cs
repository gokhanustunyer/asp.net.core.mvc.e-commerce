using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Queries.User.GetUserAddressQuery
{
    public class GetUserAddressQueryRequest: IRequest<GetUserAddressQueryResponse>
    {
        public string UserName { get; set; }
    }
}
