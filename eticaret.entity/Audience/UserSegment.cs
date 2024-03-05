using eticaret.entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Audience
{
    public class UserSegment : BaseEntity.BaseEntity
    {
        public string SegmentTitle { get; set; }
        public ICollection<AppUser> Users { get; set; }
    }
}
