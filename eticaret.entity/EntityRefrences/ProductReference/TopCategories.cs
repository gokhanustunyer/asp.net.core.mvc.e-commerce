using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.entity.EntityRefrences.ProductReference
{
    public class TopCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<TopCategory> SubCategories { get; set; }
    }
}
