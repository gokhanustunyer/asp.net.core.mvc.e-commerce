using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Contexts.AddressEntities
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<District> Districts { get; set; }
    }
}
