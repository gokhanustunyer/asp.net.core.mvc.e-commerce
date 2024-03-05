using eticaret.entity.Audience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.AudienceReferences
{
    public class UserSegmentReference
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string SegmentId { get; set; }
        public string SegmentTitle { get; set; }
    }
}
