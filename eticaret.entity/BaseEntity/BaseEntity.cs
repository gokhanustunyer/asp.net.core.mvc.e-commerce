using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.BaseEntity
{
    public class BaseEntity
    {
        public Guid Id{ get; set; } = Guid.NewGuid();
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;
    }
}
