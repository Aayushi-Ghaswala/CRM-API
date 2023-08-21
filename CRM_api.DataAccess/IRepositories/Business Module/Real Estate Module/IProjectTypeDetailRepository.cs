using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.Real_Estate_Module
{
    public interface IProjectTypeDetailRepository
    {
        Task<Response<TblProjectTypeDetail>> GetProjectTypeDetails(int? projectTypeId, string? search, SortingParams sortingParams);
        Task<Response<TblProjectTypeMaster>> GetProjectTypes(string? search, SortingParams sortingParams);
        Task<int> AddProjectTypeDetail(TblProjectTypeDetail tblProjectTypeDetail);
        Task<int> UpdateProjectTypeDetail(TblProjectTypeDetail tblProjectTypeDetail);
        Task<int> DeleteProjectTypeDetail(int id);
    }
}
