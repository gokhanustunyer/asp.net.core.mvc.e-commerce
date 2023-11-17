using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.entity.PageStrings
{
    public class PageStrings: et.BaseEntity.BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
