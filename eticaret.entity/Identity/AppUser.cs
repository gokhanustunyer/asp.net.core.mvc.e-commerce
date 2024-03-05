using eticaret.entity.Audience;
using eticaret.entity.Product;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.entity.Identity
{
    public class AppUser: IdentityUser<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Country { get; set; }
        public string? CartId { get; set; }
        public Cart.Cart? Cart { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public ICollection<et.Order.Order> Orders { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<UserSegment> Segments { get; set; }
        public ICollection<ProductRate> ProductRates { get; set; }
        public ICollection<ProductComment> Comments { get; set; }
        public ICollection<ProductQA> QAs { get; set; }
    }
}
