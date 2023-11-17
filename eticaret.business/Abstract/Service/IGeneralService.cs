using eticaret.entity.EntityRefrences.MixReferences;
using eticaret.entity.EntityRefrences.PageEntities.Mail;
using eticaret.entity.PageEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Service
{
    public interface IGeneralService
    {
        Task<AdminStatisticsModel> GetStatistics(int year, int month);
        Task<bool> SendSupportMail(SupportFormModel formModel);
        Task<List<SupportFormModel>> GetAllSupportForms();
        Task<bool> SendSupportMailResponse(string mailId, string subject, string htmlContent);
        Task<SupportMailCombModel> GetMailWithResponses(string mailId);
    }
}
