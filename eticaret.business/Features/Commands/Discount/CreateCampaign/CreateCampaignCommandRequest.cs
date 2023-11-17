using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Discount.CreateCampaign
{
    public class CreateCampaignCommandRequest: IRequest<CreateCampaignCommandResponse>
    {
        public string Code { get; set; }
        public DateTime CodeStartDate { get; set; }
        public DateTime CodeEndDate { get; set; }
        public int DiscountRate { get; set; }
        public string[] CategoryIds { get; set; }
        public string SelectedProductIds { get; set; }
    }
}
