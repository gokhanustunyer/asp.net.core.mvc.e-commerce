using eticaret.business.Abstract.Service;
using eticaret.data.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dt = eticaret.data.Services;

namespace eticaret.business.Concrete.Service
{
    public class CityDbService: ICityDbService
    {
        private readonly ICitiesDbService _citiesDbService;

        public CityDbService(ICitiesDbService citiesDbService)
        {
            _citiesDbService = citiesDbService;
        }

        public List<Tuple<int, string>> GetAllCities()
            => _citiesDbService.AllCities;

        public List<Tuple<int, string>> GetAllDistrictByCityId(int id)
            => _citiesDbService.GetDistrictByCityId(id);

        public List<Tuple<int, string>> GetAllNeighborhoodsByDistrict(int id)
            => _citiesDbService.GetNeighborhoodsByDistrictId(id);

    }
}
