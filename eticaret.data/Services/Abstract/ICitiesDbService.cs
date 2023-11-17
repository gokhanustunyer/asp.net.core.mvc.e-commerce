using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Services.Abstract
{
    public interface ICitiesDbService
    {
        List<Tuple<int, string>> AllCities { get; }
        List<Tuple<int, string>> GetDistrictByCityId(int id);
        List<Tuple<int, string>> GetNeighborhoodsByDistrictId(int id);

        string GetCityById(int id);
        string GetDistrictById(int id);
        string GetNeighborhoodById(int id);
        bool IsValid(string city, string district, string neighborhood);
    }
}
