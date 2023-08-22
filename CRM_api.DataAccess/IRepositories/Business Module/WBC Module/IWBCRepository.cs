﻿using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.WBC_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.WBC_Module
{
    public interface IWBCRepository
    {
        Task<List<TblGoldPointCategory>> GetPointCategory();
        Task<Response<UserNameResponse>> GetGPUsername(string? type, string? searchingParams, SortingParams sortingParams);
        Task<Response<WBCTypeResponse>> GetGPTypes(int? userId, string? searchingParams, SortingParams sortingParams);
        Task<LedgerResponse<TblGoldPoint>> GetGPLedgerReport(DateTime? date, int? userId, string? type, int? categoryId, string? searchingParams, SortingParams sortingParams);
        Task<Response<WbcGPResponseModel>> GetGP(string? search, DateTime date, SortingParams sortingParams);
        Task<(int, string)> ReleaseGP(DateTime date, bool isTruncate = false);
        Task<TblWbcSchemeMaster> GetWBCSchemeByWBCTypeId(int id, DateTime date);
        Task<List<TblGoldReferral>> GetUserReferalDetails(DateTime date);
        Task<Response<TblWbcTypeMaster>> GetAllWbcSchemeTypes(string? searchingParams, SortingParams sortingParams);
        Task<Response<TblWbcSchemeMaster>> GetAllWbcSchemes(string? searchingParams, SortingParams sortingParams);
        Task<Response<ReferenceTrackingResponseModel>> GetDirectRefferals(int userId, string? search, SortingParams sortingParams);
        Task<List<ReferenceTrackingResponseModel>> GetRefferedByList(int userId);
        Task<int> AddWbcScheme(TblWbcSchemeMaster wbcSchemeMaster);
        Task<int> UpdateWbcScheme(TblWbcSchemeMaster wbcSchemeMaster);
        Task<int> DeleteWbcScheme(int id);
    }
}
