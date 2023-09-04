using CRM_api.DataAccess.Models;

namespace CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module
{
    public interface IMutualfundDashBoardRepository
    {
        Task<List<GetTopTenSchemeByInvestment>> GetTopTenSchemeByInvestmentWise();
        Task<IQueryable<TblMftransaction>> GetMFInSpecificDateForExistUser(DateTime? startDate, DateTime? endDate);
        Task<List<vw_Mftransaction>> GetAllMFTransaction();
    }
}
