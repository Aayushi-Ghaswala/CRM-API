﻿using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.MGain_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module
{
    public interface IMGainRepository
    {
        Task<int> GetMonthlyMGainDetailByUserId(int userId, DateTime date);
        Task<MGainBussinessResponse<TblMgaindetail>> GetMGainDetails(int? currencyId, string? type, bool? isClosed, DateTime? fromDate, DateTime? toDate, string? searchingParams, SortingParams sortingParams, int? mgainCompanyId);
        Task<IQueryable<TblMgaindetail>> GetAllMGainDetailsMonthly(int? schemeId, string? searchingParams, SortingParams sortingParams, string mgainType, DateTime date, int? filterCompanyId);
        Task<TblMgaindetail> GetMGainDetailById(int id);
        Task<List<TblMgainPaymentMethod>> GetPaymentByMGainId(int mGainId);
        Task<TblMgainPaymentMethod> GetPaymentById(int id);
        Task<List<TblAccountTransaction>> GetMGainAccTransactionByUserId(int userId, DateTime? startDate, DateTime? endDate);
        Task<IQueryable<TblMgaindetail>> GetMGainCumulativeDetails(int fromYear, int toYear, int? schemeId, string? search, SortingParams sortingParams, string mgainType);
        Task<TblProjectMaster> GetProjectByProjectName(string projectName);
        Task<List<TblPlotMaster>> GetPlotsByProjectId(int projectId, int? plotId);
        Task<List<TblMgaindetail>> GetMGainDetailsByUserId(int UserId);
        Task<List<TblAccountTransaction>> GetAccountTransactionByMgainId(int? mGainId, int? month, int? year);
        TblAccountMaster GetAccountByUserId(int? userId, string? accountName, int companyId);
        Task<List<TblMgainCurrancyMaster>> GetAllCurrencies();
        Task<TblPlotMaster> GetPlotById(int? id);
        Task<TblPlotMaster> GetPlotByProjectAndPlotNo(string? projectName, string plotNo);
        Task<TblMgaindetail> AddMGainDetails(TblMgaindetail mgainDetail);
        Task<int> AddPaymentDetails(List<TblMgainPaymentMethod> tblMgainPayment);
        Task<int> AddUserAccount(TblAccountMaster tblAccountMaster);
        int AddMGainInterest(List<TblAccountTransaction> tblAccountTransactions, DateTime? date);
        Task<int> UpdateMGainDetails(TblMgaindetail tblMgaindetail);
        Task<int> UpdateMGainPayment(TblMgainPaymentMethod tblMgainPayment);
        Task<int> UpdatePlotDetails(List<TblPlotMaster> tblPlotMaster);
        Task<int> DeleteMGainPayment(TblMgainPaymentMethod tblMgainPayment);
        Task<int> AddMGainPlotDetails(List<TblMgainPlotData> tblMgainPlots);
        Task<int> DeleteMGainPlotDetails(int Id);
        Task<IList<TblMgainPlotData>> GetMGainPlotDetails(int mgainId);
        Task<int> AddMGainRedemptionRequest(TblMgainRedemptionRequest tblMgainRedemptionRequest);
        Task<int> UpdateMGainRedemptionRequest(TblMgainRedemptionRequest tblMgainRedemptionRequest);
        Task<int> DeleteMGainRedemptionRequest(int Id, string? reason);
        Task<Response<TblMgainRedemptionRequest>> GetAllMGainRedemptionRequest(string? searchingParams, SortingParams sortingParams);
        Task<TblMgainRedemptionRequest> GetMGainRedemptionRequestById(int Id);
        Task<Response<TblMgaindetail>> GetMGainListByClientId(int ClientId);

    }
}
