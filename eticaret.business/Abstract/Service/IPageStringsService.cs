using eticaret.entity.EntityRefrences.PageStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Service
{
    public interface IPageStringsService
    {
        List<Tuple<string, string>> GetNotices();
        Task<bool> SetNotice(UpdateNoticeReference model);
        Task<bool> DeleteNoticeAsync(string id);
    }
}
