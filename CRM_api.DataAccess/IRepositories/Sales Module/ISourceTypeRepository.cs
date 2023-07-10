using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Sales_Module
{
    public interface ISourceTypeRepository
    {
        Task<Response<TblSourceTypeMaster>> GetSourceTypes(string search, SortingParams sortingParams);
        Task<TblSourceTypeMaster> GetSourceTypeById(int id);
        Task<TblSourceTypeMaster> GetSourceTypeByName(string Name);
        Task<int> AddSourceType(TblSourceTypeMaster status);
        Task<int> UpdateSourceType(TblSourceTypeMaster status);
        Task<int> DeactivateSourceType(int id);
    }
}
