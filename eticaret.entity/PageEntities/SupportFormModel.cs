using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.PageEntities
{
    public class SupportFormModel: BaseEntity.BaseEntity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Messsage { get; set; }
    }
}
