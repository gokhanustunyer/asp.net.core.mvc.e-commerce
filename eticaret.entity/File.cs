using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity.BaseEntity;

namespace eticaret.entity
{
    public class File: et.BaseEntity
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Storage { get; set; }
    }
}
