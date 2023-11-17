using eticaret.entity.EntityRefrences.FilterReference;
using eticaret.entity.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Service
{
    public interface IFilterService
    {
        Task<List<FilterBox>> GetFilterByCategoryAsync(List<string> categoryIds);
        Task<List<FilterBox>> GetFilterByCategoryIdAsync(string categoryId);
        Task<FilterEditModel> GetForEditById(string id);
        Task<List<Filter>> GetFiltersByFilterBoxId(string filterBoxId);
        Task<bool> UpdateFilterName(string filterId, string filterName);
        Task<bool> AddNewFilterToFilterBox(string filterBoxId, string filterName);
        Task<bool> DeleteFilterFromFilterBox(string filterBoxId, string filterId);
        Task<FilterDetailsModel> GetFilterDetailsModelById(string filterId);
        Task<bool> RemoveProductFromFilter(string filterId, string productId);
        Task<bool> UpdateFilterBoxName(string filterBoxId, string filterBoxName);
        Task<bool> DeleteFilterBox(string filterBoxId);
        Task<bool> UpdateFilterBox(string filterBoxId, string filterBoxName, string[] categoryIds);
    }
}
