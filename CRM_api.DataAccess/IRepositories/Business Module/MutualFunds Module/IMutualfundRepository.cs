using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.MutualFunds_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module
{
    public interface IMutualfundRepository
    {
        Task<List<TblMftransaction>> GetMFInSpecificDateForExistUser(DateTime? startDate, DateTime? endDate, int userId = 0);
        Task<List<TblMftransaction>> GetMFInSpecificDateForExistUser(DateTime? startDate, DateTime? endDate);
        Task<List<TblNotexistuserMftransaction>> GetMFInSpecificDateForNotExistUser(DateTime? startDate, DateTime? endDate);
        int GetSchemeIdBySchemeName(string schemeName);
        Task<BussinessResponse<TblMftransaction>> GetTblMftransactions(int userId, string? schemeName, string? folioNo
            , string? searchingParams, SortingParams sortingParams, DateTime? startDate, DateTime? endDate);
        Task<List<IGrouping<string?, TblMftransaction>>> GetMFTransactionSummary(int userId);
        Task<Response<UserNameResponse>> GetMFUserName(string? searchingParams, SortingParams sortingParams);
        Task<Response<TblMftransaction>> GetSchemeName(int userId, string? folioNo, string? searchingParams, SortingParams sortingParams);
        Task<Response<TblMftransaction>> GetFolioNo(int userId, string? schemeName, string? searchingParams, SortingParams sortingParams);
        Task<List<IGrouping<string?, TblMftransaction>>> GetMFTransactionSummaryByCategory(int userId);
        Task<List<IGrouping<string?, TblMftransaction>>> GetAllCLientMFSummary(DateTime fromDate, DateTime toDate);
        Task<List<TblMfSchemeMaster>> GetAllMFScheme();
        Task<List<TblMftransaction>> GetMFTransactionsByUserIds(List<int?> userIds, DateTime fromDate, DateTime toDate);
        Task<(List<TblAmfiNav>, Response<TblAmfiNav>)> GetAMFINavList(bool withSorting, string? search, SortingParams sortingParams);
        Task<(List<TblAmfiSchemeMaster>, Response<TblAmfiSchemeMaster>)> GetAMFISchemesList(bool withSorting, string? search, SortingParams sortingParams);
        Task<int> AddMFDataForExistUser(List<TblMftransaction> tblMftransaction);
        Task<int> AddMFDataForNotExistUser(List<TblNotexistuserMftransaction> tblNotexistuserMftransaction);
        Task<int> UpdateMFScheme(List<TblMfSchemeMaster> schemeMasters);
        Task<int> UpdateAMFINav(List<TblAmfiNav> amfiNavs);
        Task<int> UpdateAMFISchemes(List<TblAmfiSchemeMaster> tblAmfiSchemes);
        Task<int> DeleteMFForUserExist(TblMftransaction tblMftransaction);
        Task<int> DeleteMFForNotUserExist(TblNotexistuserMftransaction tblMftransaction);
        Task<decimal?> GetMFTransactionByUserId(int userId);
        Task<List<TblMftransaction>> GetMonthlyMFTransactionSIPLumpsum();
        Task<List<TblMftransaction>> GetMFTransactionSIPLumpsumByUserId(int? month, int? year, int userId);
        Task<int> DeleteMFForUserExistRange(IList<TblMftransaction> tblMftransaction);
        Task<int> DeleteMFForNotUserExistRange(IList<TblNotexistuserMftransaction> tblMftransaction);
    }
}
