using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.Product
{
    public class OptionModel
    {
        public Size? Size { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double? Price { get; set; }
    }
}
