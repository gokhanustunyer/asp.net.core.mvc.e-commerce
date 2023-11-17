using eticaret.data.Abstract;
using eticaret.data.Abstract.Product;
using eticaret.data.Contexts;
using eticaret.data.Services.Abstract;
using eticaret.entity.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Features.Commands.User.AddAddress
{
    public class AddAddressCommandHandler
        : IRequestHandler<AddAddressCommandRequest, AddAddressCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICitiesDbService _citiesDbService;
        private readonly ETicaretDbContext _eTicaretDbContext;

        public AddAddressCommandHandler(UserManager<AppUser> userManager,
                                        ICitiesDbService citiesDbService,
                                        ETicaretDbContext eTicaretDbContext)
        {
            _userManager = userManager;
            _citiesDbService = citiesDbService;
            _eTicaretDbContext = eTicaretDbContext;
        }

        public async Task<AddAddressCommandResponse> Handle
            (AddAddressCommandRequest request, CancellationToken cancellationToken)
        {
            string city, district, neighborhood;
            AppUser   user;
            city = _citiesDbService.GetCityById(request.CityId);
            district = _citiesDbService.GetDistrictById(request.DistrictId);
            neighborhood = _citiesDbService.GetNeighborhoodById(request.NeighborhoodId);

            user = await _userManager.FindByNameAsync(request.UserName);
            if (!_citiesDbService.IsValid(city, district, neighborhood)) { return new(); };
            if (request.AddressId != "null" && request.AddressId != null)
            {
                Address address = _eTicaretDbContext.Addresses
                                  .FirstOrDefault(a => a.Id == Guid.Parse(request.AddressId));
                address.Neighborhood = neighborhood;
                address.District = district;
                address.City = city;
                address.UpdateDate = DateTime.Now;
                address.DetailedAddress = request.DetailedAddress;
                address.Title = request.Title;
                _eTicaretDbContext.Addresses.Update(address);
            }
            else
            {
                await _userManager.UpdateAsync(user);
                _eTicaretDbContext.Addresses.Add(new Address()
                {
                    DetailedAddress = request.DetailedAddress,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Title = request.Title,
                    Id = Guid.NewGuid(),
                    User = user,
                    Neighborhood = neighborhood,
                    District = district,
                    City = city,
                });
            }
            _eTicaretDbContext.SaveChanges();
            return new();
        }
    }
}
