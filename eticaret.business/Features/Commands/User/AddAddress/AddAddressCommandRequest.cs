using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.User.AddAddress
{
    public class AddAddressCommandRequest: IRequest<AddAddressCommandResponse>
    {
        public string AddressId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int NeighborhoodId { get; set; }
        public string DetailedAddress { get; set; }
    }
}
