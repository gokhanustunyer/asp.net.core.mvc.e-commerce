using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.PageEntities
{
    public class SupportMailResponseModel: BaseEntity.BaseEntity
    {
        public SupportFormModel SupportMail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
