using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Service
{
    public interface ICityDbService
    {
        List<Tuple<int, string>> GetAllCities();
        List<Tuple<int, string>> GetAllDistrictByCityId(int id);
        List<Tuple<int, string>> GetAllNeighborhoodsByDistrict(int id);
    }
}
