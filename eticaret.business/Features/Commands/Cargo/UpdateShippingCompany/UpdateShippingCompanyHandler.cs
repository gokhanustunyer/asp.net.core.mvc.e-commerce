using eticaret.business.Abstract.Storage.Azure;
using eticaret.data.Abstract.Shipping;
using eticaret.data.Contexts;
using eticaret.entity.Cargo;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.Cargo.UpdateShippingCompany
{
    public class UpdateShippingCompanyHandler : IRequestHandler<UpdateShippingCompanyRequest, UpdateShippingCompanyResponse>
    {
        private readonly IShippingRepository _shippingRepository;

        public UpdateShippingCompanyHandler(IShippingRepository shippingRepository)
        {
            _shippingRepository = shippingRepository;
        }

        public async Task<UpdateShippingCompanyResponse> Handle(UpdateShippingCompanyRequest request, CancellationToken cancellationToken)
        {
            Shipping company = _shippingRepository.Table.FirstOrDefault(c => c.Id.ToString() == request.Id);

            company.Name = request.Name;
            company.Price = request.Price;
            company.Description = request.Description;
            if (request.IsDefault)
            {
                Shipping defaultCompany = _shippingRepository.Table.FirstOrDefault(c => c.IsDefault);
                if (defaultCompany != null)
                {
                    defaultCompany.IsDefault = false;
                    _shippingRepository.Update(defaultCompany);
                    company.IsDefault = request.IsDefault;
                }
            }
            _shippingRepository.Update(company);
            await _shippingRepository.SaveAsync();
            return new();
        }
    }
}
