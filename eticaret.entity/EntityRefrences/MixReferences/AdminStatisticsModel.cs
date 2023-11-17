using eticaret.entity.EntityRefrences.IdentityReference;
using eticaret.entity.EntityRefrences.ProductReference;
using eticaret.entity.Identity;
using eticaret.entity.PageEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.entity.EntityRefrences.MixReferences
{
    public class AdminStatisticsModel
    {
        public Dictionary<string,    int> OrderCountByCategoryName { get; set; }
        public Dictionary<string,    int> TotalSalesCountByDay { get; set; }
        public Dictionary<string,    int> VisitCountForaWeek { get; set; }
        public Dictionary<string, double> OrderPricesByCategoryName { get; set; }
        public Dictionary<string, double> TotalSalesPriceByDay { get; set; }
        public List<ProductListModel> RecentlySoldItems { get; set; }
        public List<ProductListModel> TopSellingItems { get; set; }
        public List<SupportFormModel> LastUserMessages { get; set; }
        public List<ProductCommentModel> RecentlyReviews { get; set; }
        public List<UserModel> RecentRegisters { get; set; }
        public List<UserModel> RecentLogs { get; set; }
        public int DailyOrderCount { get; set; }
        public int DailyRegisterCount { get; set; }
        public int DailyVisitCount { get; set; }
        public double DailySalary { get; set; }
    }
}
