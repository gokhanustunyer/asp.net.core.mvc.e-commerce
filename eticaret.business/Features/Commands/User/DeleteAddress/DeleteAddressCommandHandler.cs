using eticaret.data.Contexts;
using eticaret.entity.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.User.DeleteAddress
{
    public class DeleteAddressCommandHandler
        : IRequestHandler<DeleteAddressCommandRequest, DeleteAddressCommandResponse>
    {
        private readonly ETicaretDbContext _eTicaretDbContext;

        public DeleteAddressCommandHandler(ETicaretDbContext eTicaretDbContext)
        {
            _eTicaretDbContext = eTicaretDbContext;
        }

        public async Task<DeleteAddressCommandResponse> Handle
            (DeleteAddressCommandRequest request, CancellationToken cancellationToken)
        {
            Address address = _eTicaretDbContext.Addresses
                    .FirstOrDefault(a => a.Id == Guid.Parse(request.AddressId));
            _eTicaretDbContext.Remove(address);

            await _eTicaretDbContext.SaveChangesAsync();
            return new();        
        }
    }
}
