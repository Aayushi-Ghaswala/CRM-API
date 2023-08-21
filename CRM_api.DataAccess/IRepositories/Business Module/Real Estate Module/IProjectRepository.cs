using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.Real_Estate_Module
{
    public interface IProjectRepository
    {
        Task<Response<TblProjectMaster>> GetProjects(bool? isActive, string? search, SortingParams sortingParams);
        Task<int> AddProject(TblProjectMaster projectMaster);
        Task<int> UpdateProject(TblProjectMaster projectMaster);
        Task<int> DeactivateProject(int id);
    }
}
