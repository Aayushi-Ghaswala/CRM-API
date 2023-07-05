using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.MGain_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module
{
    public interface IMGainRepository
    {
        Task<MGainBussinessResponse<TblMgaindetail>> GetMGainDetails(int? currencyId, string? type, bool? isClosed, DateTime? fromDate, DateTime? toDate, string? searchingParams, SortingParams sortingParams);
        Task<IQueryable<TblMgaindetail>> GetAllMGainDetailsMonthly(int? schemeId, string? searchingParams, SortingParams sortingParams, string mgainType, DateTime date);
        Task<TblMgaindetail> GetMGainDetailById(int id);
        Task<List<TblMgainPaymentMethod>> GetPaymentByMGainId(int mGainId);
        Task<TblMgainPaymentMethod> GetPaymentById(int id);
        Task<List<TblAccountTransaction>> GetMGainAccTransactionByUserId(int userId, int year);
        Task<IQueryable<TblMgaindetail>> GetMGainCumulativeDetails(int fromYear, int toYear, int? schemeId, string? search, SortingParams sortingParams, string mgainType);
        Task<Response<TblProjectMaster>> GetAllProject(string? searchingParams, SortingParams sortingParams);
        Task<TblProjectMaster> GetProjectByProjectName(string projectName);
        Task<Response<TblPlotMaster>> GetPlotsByProjectId(int projectId, int? plotId, string? searchingParams, SortingParams sortingParams);
        Task<List<TblMgaindetail>> GetMGainDetailsByUserId(int UserId);
        Task<List<TblAccountTransaction>> GetAccountTransactionByMgainId(int? mGainId, int? month, int? year);
        Task<TblAccountMaster> GetAccountByUserId(int? userId, string? accountName);
        Task<List<TblMgainCurrancyMaster>> GetAllCurrencies();
        Task<TblPlotMaster> GetPlotById(int? id);
        Task<TblPlotMaster> GetPlotByProjectAndPlotNo(string? projectName, string plotNo);
        Task<TblMgaindetail> AddMGainDetails(TblMgaindetail mgainDetail);
        Task<int> AddPaymentDetails(List<TblMgainPaymentMethod> tblMgainPayment);
        Task<int> AddUserAccount(TblAccountMaster tblAccountMaster);
        Task<int> AddMGainInterest(List<TblAccountTransaction> tblAccountTransactions, DateTime? date);
        Task<int> UpdateMGainDetails(TblMgaindetail tblMgaindetail);
        Task<int> UpdateMGainPayment(TblMgainPaymentMethod tblMgainPayment);
        Task<int> UpdatePlotDetails(List<TblPlotMaster> tblPlotMaster);
        Task<int> DeleteMGainPayment(TblMgainPaymentMethod tblMgainPayment);
    }
}
