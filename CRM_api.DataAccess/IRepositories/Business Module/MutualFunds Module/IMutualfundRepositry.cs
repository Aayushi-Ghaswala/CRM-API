using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module
{
    public interface IMutualfundRepositry
    {
        Task<int> AddMFDataForExistUser(List<TblMftransaction> tblMftransaction);
        Task<int> AddMFDataForNotExistUser(List<TblNotexistuserMftransaction> tblNotexistuserMftransaction);
        Task<List<TblMftransaction>> GetMFInSpecificDateForExistUser(DateTime? StartDate, DateTime? EndDate);
        Task<List<TblNotexistuserMftransaction>> GetMFInSpecificDateForNotExistUser(DateTime? StartDate, DateTime? EndDate);
        Task<int> DeleteMFForUserExist(TblMftransaction tblMftransaction);
        Task<int> DeleteMFForNotUserExist(TblNotexistuserMftransaction tblMftransaction);
        int GetSchemeIdBySchemeName(string schemeName);
    }
}
