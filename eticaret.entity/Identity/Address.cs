using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;


namespace eticaret.entity.Identity
{
    public class Address: et.BaseEntity.BaseEntity
    {
        public AppUser User { get; set; }
        public string? PhoneNumber { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Neighborhood { get; set; }
        public string DetailedAddress { get; set; }
        public string? PostCode { get; set; }
    }
}
