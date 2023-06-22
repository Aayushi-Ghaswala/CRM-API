using CRM_api.DataAccess.Models;

namespace CRM_api.DataAccess.IRepositories.Business_Module.Fasttrack_Module
{
    public interface IFasttrackRepository
    {
        Task<string> GetUserFasttrackCategory(int userId);
        Task<int> UpdateFasttrackScheme(TblFasttrackSchemeMaster tblFasttrackSchemeMaster);
        Task<int> UpdateFasttrackLevelsCommission(TblFasttrackLevelCommission tblFasttrackLevelCommission);
    }
}
