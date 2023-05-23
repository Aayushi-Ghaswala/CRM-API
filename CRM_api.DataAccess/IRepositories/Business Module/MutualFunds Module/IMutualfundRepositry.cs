using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module
{
    public interface IMutualfundRepositry
    {
        Task<int> AddMutualfundDetailsForExistUser(List<TblMftransaction> tblMftransaction);
        Task<int> AddMutualfundDetailsToNotExistUserTable(List<TblNotexistuserMftransaction> tblNotexistuserMftransaction);
        Task<List<TblMftransaction>> GetMutualfundInSpecificDateForExistUser(DateTime? StartDate, DateTime? EndDate);
        Task<List<TblNotexistuserMftransaction>> GetMutualfundInSpecificDateForNotExistUser(DateTime? StartDate, DateTime? EndDate);
        Task<int> DeleteMutualFundInUserExist(TblMftransaction tblMftransaction);
        Task<int> DeleteMutualFundInNotUserExist(TblNotexistuserMftransaction tblMftransaction);
        int GetSchemeIdBySchemeName(string schemeName);
    }
}
