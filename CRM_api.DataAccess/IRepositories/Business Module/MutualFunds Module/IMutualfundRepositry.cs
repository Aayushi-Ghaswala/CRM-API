using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module
{
    public interface IMutualfundRepositry
    {
        Task<List<TblMftransaction>> GetMFInSpecificDateForExistUser(DateTime? StartDate, DateTime? EndDate);
        Task<List<TblNotexistuserMftransaction>> GetMFInSpecificDateForNotExistUser(DateTime? StartDate, DateTime? EndDate);
        int GetSchemeIdBySchemeName(string schemeName);
        Task<int> AddMFDataForExistUser(List<TblMftransaction> tblMftransaction);
        Task<int> AddMFDataForNotExistUser(List<TblNotexistuserMftransaction> tblNotexistuserMftransaction);
        Task<int> DeleteMFForUserExist(TblMftransaction tblMftransaction);
        Task<int> DeleteMFForNotUserExist(TblNotexistuserMftransaction tblMftransaction);
    }
}
