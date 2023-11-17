using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Cart.Checkout
{
    public class CheckoutCommandRequest: IRequest<CheckoutCommandResponse>
    {
        public string NameOnCart { get; set; }
        public string CartNumber { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string CVCCode { get; set; }
        public string AddressId { get; set; }
        public string UserName { get; set; }
    }
}
