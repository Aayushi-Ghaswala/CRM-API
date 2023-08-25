using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.Fasttrack_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.Fasttrack_Module
{
    public interface IFasttrackRepository
    {
        Task<Response<FasttrackInvTypeResponse>> GetFtSubInvTypes(int? invTypeId, string? search, SortingParams sortingParams);
        Task<Response<FasttrackInvTypeResponse>> GetFtInvTypes(int? userId, string? search, SortingParams sortingParams);
        Task<Response<UserNameResponse>> GetFtUsername(int? typeId, int? subTypeId, string? search, SortingParams sortingParams);
        Task<LedgerResponse<TblFasttrackLedger>> GetFasttrackLedgerReport(int? userId, int? typeId, int? subTypeId, string? search, SortingParams sortingParams);
        Task<List<FasttrackResponseModel>> GetFasttrackCommissionView(DateTime date);
        Task<(int, string, List<FasttrackResponseModel>?)> ReleaseCommission(DateTime date, bool isTruncate);
        Task<string> GetUserFasttrackCategory(int userId);
        Task<List<TblFasttrackBenefits>> GetFasttrackBenefits();
        Task<List<TblFasttrackSchemeMaster>> GetFasttrackSchemes();
        Task<List<TblFasttrackLevelCommission>> GetFasttrackLevelCommission();
        Task<int> AddFasttrackBenefit(TblFasttrackBenefits tblFasttrackBenefits);
        Task<int> UpdateFasttrackBenefit(TblFasttrackBenefits tblFasttrackBenefits);
        Task<int> UpdateFasttrackScheme(TblFasttrackSchemeMaster tblFasttrackSchemeMaster);
        Task<int> UpdateFasttrackLevelsCommission(TblFasttrackLevelCommission tblFasttrackLevelCommission);
    }
}
