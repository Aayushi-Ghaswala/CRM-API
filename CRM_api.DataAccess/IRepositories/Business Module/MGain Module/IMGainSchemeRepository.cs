using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module
{
    public interface IMGainSchemeRepository
    {
        Task<Response<TblMgainSchemeMaster>> GetMGainSchemeDetails(bool? IsActive, string? searchingParamas, SortingParams sortingParams);
        Task<int> AddMGainScheme(TblMgainSchemeMaster tblMgainSchemeMaster);
        Task<int> UpdateMGainScheme(TblMgainSchemeMaster tblMgainSchemeMaster);
        Task<TblMgainSchemeMaster> GetMGainSchemeById(int id);
    }
}
