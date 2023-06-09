using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.MGain_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module
{
    public interface IMGainRepository
    {
        Task<MGainBussinessResponse<TblMgaindetail>> GetMGainDetails(int? currancyId, string? type, bool? isClosed, DateTime? fromDate, DateTime? toDate, string? searchingParams, SortingParams sortingParams);
        Task<TblMgaindetail> GetMGainDetailById(int id);
        Task<List<TblMgainPaymentMethod>> GetPaymentByMGainId(int mGainId);
        Task<TblMgainPaymentMethod> GetPaymentById(int id);
        Task<Response<TblProjectMaster>> GetAllProject(string? searchingParams, SortingParams sortingParams);
        Task<TblProjectMaster> GetProjectByProjectName(string projectName);
        Task<Response<TblPlotMaster>> GetPlotsByProjectId(int projectId, decimal invAmount, string? searchingParams, SortingParams sortingParams);
        Task<List<TblMgainCurrancyMaster>> GetAllCurrencies();
        Task<TblPlotMaster> GetPlotById(int id);
        Task<TblPlotMaster> GetPlotByProjectAndPlotNo(decimal? totalSqFt, string plotNo);
        Task<TblMgaindetail> AddMGainDetails(TblMgaindetail mgainDetail);
        Task<int> AddPaymentDetails(List<TblMgainPaymentMethod> tblMgainPayment);
        Task<int> UpdateMGainDetails(TblMgaindetail tblMgaindetail);
        Task<int> UpdateMGainPayment(TblMgainPaymentMethod tblMgainPayment);
        Task<int> UpdatePlotDetails(TblPlotMaster tblPlotMaster);
        Task<int> DeleteMGainPayment(TblMgainPaymentMethod tblMgainPayment);
    }
}
