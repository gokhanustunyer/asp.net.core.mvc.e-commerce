using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Contexts.AddressEntities
{
    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public City City { get; set; }
    }
}
