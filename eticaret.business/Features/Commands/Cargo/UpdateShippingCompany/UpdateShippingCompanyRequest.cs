using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Cargo.UpdateShippingCompany
{
    public class UpdateShippingCompanyRequest: IRequest<UpdateShippingCompanyResponse>
    {
        public IFormFileCollection CompanyLogo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsDefault { get; set; }
        public string Id { get; set; }
    }
}
