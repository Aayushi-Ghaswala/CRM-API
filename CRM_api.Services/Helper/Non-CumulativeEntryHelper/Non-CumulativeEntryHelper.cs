using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module;
using CRM_api.Services.IServices.Account_Module;
using CRM_api.Services.IServices.Business_Module.MGain_Module;

namespace CRM_api.Services.Helper.Non_CumulativeEntryHelper
{
    public static class Non_CumulativeEntryHelper
    {
        public static async void EntryHelper(IMGainRepository mGainRepository, IMGainService iMGainService, IAccountTransactionservice accountTransactionservice)
        {
            DateTime? date = Convert.ToDateTime(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString() + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year);
            SortingParams sortingParams = new SortingParams();
            sortingParams.SortBy = "id";

            await iMGainService.GetNonCumulativeMonthlyReportAsync(DateTime.Now.Month, DateTime.Now.Year, null, 10, false, null, null, null, sortingParams, false, null,true, "M Gain Interest", date);
        }
    }
}
