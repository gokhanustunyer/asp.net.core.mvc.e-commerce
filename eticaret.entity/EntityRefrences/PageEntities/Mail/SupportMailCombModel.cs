using eticaret.entity.PageEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.PageEntities.Mail
{
    public class SupportMailCombModel
    {
        public SupportFormModel SupportForm { get; set; }
        public List<SupportMailResponseModel> SupportFormResponses { get; set; }
    }
}
