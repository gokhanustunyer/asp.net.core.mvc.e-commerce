using eticaret.entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Queries.User.GetUserAddressQuery
{
    public class GetUserAddressQueryResponse
    {
        public List<Address> Addresses  { get; set; }
    }
}
