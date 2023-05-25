using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.MutualFunds_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module
{
    public interface IMutualfundRepositry
    {
        Task<List<TblMftransaction>> GetMFInSpecificDateForExistUser(DateTime? StartDate, DateTime? EndDate);
        Task<List<TblNotexistuserMftransaction>> GetMFInSpecificDateForNotExistUser(DateTime? StartDate, DateTime? EndDate);
        int GetSchemeIdBySchemeName(string schemeName);
        Task<BussinessResponse<TblMftransaction>> GetTblMftransactions(int userId, int? schemeId
            , string? searchingParams, SortingParams sortingParams, DateTime? StartDate, DateTime? EndDate);
        Task<List<IGrouping<string?, TblMftransaction>>> GetMFTransactionSummary(int userId);
        Task<Response<TblMftransaction>> GetSchemeName(int userId, string? searchingParams, SortingParams sortingParams);
        Task<List<IGrouping<string?, TblMftransaction>>> GetMFTransactionSummaryByCategory(int userId);
        Task<List<IGrouping<string?, TblMftransaction>>> GetAllCLientMFSummary(DateTime FromDate, DateTime ToDate);
        Task<int> AddMFDataForExistUser(List<TblMftransaction> tblMftransaction);
        Task<int> AddMFDataForNotExistUser(List<TblNotexistuserMftransaction> tblNotexistuserMftransaction);
        Task<int> DeleteMFForUserExist(TblMftransaction tblMftransaction);
        Task<int> DeleteMFForNotUserExist(TblNotexistuserMftransaction tblMftransaction);
    }
}
