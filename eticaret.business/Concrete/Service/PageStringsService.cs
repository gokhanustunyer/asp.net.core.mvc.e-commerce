using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.PageStrings;
using eticaret.entity.EntityRefrences.PageStrings;
using eticaret.entity.PageStrings;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Concrete.Service
{
    public class PageStringsService : IPageStringsService
    {
        private readonly IPageStringsRepository _pageStringRepository;

        public PageStringsService(IPageStringsRepository pageStringRepository)
        {
            _pageStringRepository = pageStringRepository;
        }

        public async Task<bool> DeleteNoticeAsync(string id)
        {
            bool result = await _pageStringRepository.RemoveAsync(id);
            await _pageStringRepository.SaveAsync();
            return result;
        }

        public List<Tuple<string, string>> GetNotices()
        {
            var notices = _pageStringRepository
                                             .Table
                                             .Where(n => n.Key == "Notice")
                                             .OrderBy(n => n.UpdateDate)
                                             .Select(n => Tuple.Create(n.Value, n.Id.ToString()))
                                             .ToList();
            return notices;
        }

        public async Task<bool> SetNotice(UpdateNoticeReference model)
        {
            PageStrings notice = await _pageStringRepository.GetByIdAsync(model.Id);
            if (notice == null)
            {
                notice = new() 
                { CreateDate = DateTime.Now,
                  UpdateDate = DateTime.Now,
                  Id = Guid.NewGuid(),
                  Key = "Notice",
                  Value = model.Value 
                };
                await _pageStringRepository.AddAsync(notice);
            }
            else
            {
                notice.Value = model.Value;
                notice.UpdateDate = DateTime.Now;
                _pageStringRepository.Update(notice);
            }

            await _pageStringRepository.SaveAsync();
            return true;
        }
    }
}
