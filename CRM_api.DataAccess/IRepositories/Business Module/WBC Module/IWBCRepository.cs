using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.WBC_Module
{
    public interface IWBCRepository
    {
        Task<Response<TblWbcTypeMaster>> GetAllWbcSchemeTypes(string? searchingParams, SortingParams sortingParams);
        Task<Response<TblSubInvesmentType>> GetAllSubInvestmentTypes(string? searchingParams, SortingParams sortingParams);
        Task<Response<TblSubsubInvType>> GetAllSubSubInvestmentTypes(string? searchingParams, SortingParams sortingParams, int? subInvestmentTypeId);
        Task<Response<TblWbcSchemeMaster>> GetAllWbcSchemes(string? searchingParams, SortingParams sortingParams);
        Task<int> AddWbcScheme(TblWbcSchemeMaster wbcSchemeMaster);
        Task<int> UpdateWbcScheme(TblWbcSchemeMaster wbcSchemeMaster);
        Task<int> DeleteWbcScheme(int id);
    }
}
