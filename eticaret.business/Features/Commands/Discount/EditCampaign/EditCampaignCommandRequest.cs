using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Discount.EditCampaign
{
    public class EditCampaignCommandRequest: IRequest<EditCampaignCommandResponse>
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public DateTime CodeStartDate { get; set; }
        public DateTime CodeEndDate { get; set; }
        public int CodeLimitNumber { get; set; }
        public int DiscountRate { get; set; }
        public double DiscountNumber { get; set; }
        public string SelectedProductIds { get; set; }
    }
}
